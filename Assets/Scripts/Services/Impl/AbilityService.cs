using System;
using CyberNinja.Config;
using CyberNinja.Ecs.Components;
using CyberNinja.Ecs.Components.Ability;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Ability;
using CyberNinja.Models.Enums;
using CyberNinja.Utils;
using CyberNinja.Views;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using UniRx;
using UnityEngine;

namespace CyberNinja.Services.Impl
{
    public class AbilityService : IAbilityService, IDestroyable
    {
        private readonly EcsWorld _world;
        private readonly UnitConfig _unitConfig;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly EcsPool<AbilityEnabledComponent> _enabledPool;
        private readonly LayersConfig _layersConfig;
        private readonly IUnitService _unitService;
        private readonly IDoorService _doorService;
        private readonly IVfxService _vfxService;
        private readonly IItemService _itemService;

        public AbilityService(EcsWorld world, UnitConfig unitConfig, LayersConfig layersConfig,
            IUnitService unitService, IDoorService doorService, IVfxService vfxService, IItemService itemService)
        {
            _world = world;
            _unitConfig = unitConfig;
            _layersConfig = layersConfig;
            _unitService = unitService;
            _doorService = doorService;
            _vfxService = vfxService;
            _itemService = itemService;

            _enabledPool = _world.GetPool<AbilityEnabledComponent>();
        }

        public void CreateAbility(AbilityItem abilityItem, int ownerEntity)
        {
            var entity = _world.NewEntity();
            var ability = new AbilityComponent
            {
                SlotIndex = abilityItem.slotIndex,
                AbilityConfig = abilityItem.abilityConfig,
                Owner = _world.PackEntity(ownerEntity)
            };

            _world.GetPool<AbilityComponent>().Add(entity) = ability;
        }

        public (bool, int) TryActivateAbility(int slotIndex, int unitEntity)
        {
            if (_unitService.HasState(unitEntity, EUnitState.Knockout))
                return (false, 0);

            var abilityEntity = 0;
            var success = false;
            var filter = _world.Filter<AbilityComponent>().End();
            var pool = _world.GetPool<AbilityComponent>();

            foreach (var entity in filter)
            {
                var ability = pool.Get(entity);

                if (!ability.Owner.Unpack(_world, out var ownerEntity))
                    continue;

                if (ownerEntity == unitEntity && ability.SlotIndex == slotIndex)
                {
                    abilityEntity = entity;
                    success = true;
                    break;
                }
            }

            if (!success)
            {
                Debug.Log("slot is empty");
            }
            else if (_world.GetPool<AbilityCooldownComponent>().Has(abilityEntity))
            {
                Debug.Log("Ability has cooldown");
            }
            else if (_world.GetPool<AbilityBlockedComponent>().Has(abilityEntity))
            {
                Debug.Log("Ability is blocked");
            }
            else
            {
                ActivateAbility(abilityEntity);
            }

            return (true, abilityEntity);
        }

        private void ActivateAbility(int abilityEntity)
        {
            var ability = _world.GetPool<AbilityComponent>().Get(abilityEntity);
            if (!ability.Owner.Unpack(_world, out var ownerEntity))
                return;

            if (_unitService.HasState(ownerEntity, EUnitState.Dash))
                return;

            if (ability.AbilityConfig.inputBlockable)
            {
                var pool = _world.GetPool<AbilityInputBlockComponent>();
                if (pool.Has(ownerEntity))
                    return;

                pool.Add(ownerEntity).Value = _unitConfig.abilityInputBlockTime;
            }

            var energyPool = _world.GetPool<EnergyComponent>();
            if (energyPool.Has(ownerEntity) && ability.AbilityConfig.energyCost > 0)
            {
                ref var energy = ref _world.GetPool<EnergyComponent>().Get(ownerEntity);
                var cost = ability.AbilityConfig.energyCost;

                if (energy.Current >= cost)
                {
                    var newEnergy = energy.Current - cost;
                    _unitService.UpdateEnergy(ownerEntity, newEnergy);
                }
                else
                    return;
            }

            if (ability.AbilityConfig.charge != 0)
            {
                Observable.Timer(TimeSpan.FromSeconds(ability.AbilityConfig.charge))
                    .Subscribe(_ =>
                    {
                        if (IsEnabled(abilityEntity))
                            SelectAbility(abilityEntity);
                    })
                    .AddTo(_disposable);
            }
            else
                SelectAbility(abilityEntity);

            if (ability.AbilityConfig.cooldown > 0)
            {
                var abilityCooldown = new AbilityCooldownComponent
                {
                    Value = ability.AbilityConfig.duration + ability.AbilityConfig.cooldown
                };
                _world.GetPool<AbilityCooldownComponent>().Add(abilityEntity) = abilityCooldown;
            }

            if (ability.AbilityConfig.VFX)
            {
                Observable.Timer(TimeSpan.FromSeconds(ability.AbilityConfig.vfxSpawnDelay))
                    .Subscribe(_ => _vfxService.SpawnVfx(ownerEntity, ability.AbilityConfig))
                    .AddTo(_disposable);
            }
            
            if (ability.AbilityConfig.ANIMATOR)
            {
                var unit = _unitService.GetUnit(ownerEntity);
                unit.View.Animator.TriggerAnimations(ability.AbilityConfig);

                if (ability.AbilityConfig.animUseUpperBodyWeight)
                {
                    if (_world.GetPool<LayerUpperBodyComponent>().Has(ownerEntity))
                    {
                        if (_world.GetPool<LayerUpperBodyTimeComponent>().Has(ownerEntity))
                        {
                            ref var layerUpperBodyTime = ref _world.GetPool<LayerUpperBodyTimeComponent>().Get(ownerEntity);
                            if (ability.AbilityConfig.animUpperBodyWeightTime > layerUpperBodyTime.Value)
                                layerUpperBodyTime.Value = ability.AbilityConfig.animUpperBodyWeightTime;
                        }
                        else
                            _world.GetPool<LayerUpperBodyTimeComponent>().Add(ownerEntity).Value =
                                ability.AbilityConfig.animUpperBodyWeightTime;
                    }
                    else
                    {
                        _world.GetPool<LayerUpperBodyComponent>().Add(ownerEntity);
                        _world.GetPool<LayerUpperBodyTimeComponent>().Add(ownerEntity).Value =
                            ability.AbilityConfig.animUpperBodyWeightTime;
                    }
                }
            }

            if (ability.AbilityConfig.STATIONARY)
            {
                if (_unitService.HasState(ownerEntity, EUnitState.Stationary))
                {
                    ref var stationary = ref _world.GetPool<StationaryComponent>().Get(ownerEntity);
                    if (ability.AbilityConfig.stationaryTime > stationary.Time)
                        stationary.Time = ability.AbilityConfig.stationaryTime;
                }
                else
                    _unitService.AddState(ownerEntity, EUnitState.Stationary, ability.AbilityConfig.stationaryTime);
            }
            
            if (ability.AbilityConfig.SHIELD)
            {
                ref var damageFactor = ref _world.GetPool<DamageFactorComponent>().Get(ownerEntity);
                damageFactor.ImpactList.Add(ability.AbilityConfig.shieldDamagePhysicalFactor);

                Observable.Timer(
                        TimeSpan.FromSeconds(ability.AbilityConfig.charge + ability.AbilityConfig.shieldDamagePhysicalTime))
                    .Subscribe(_ =>
                    {
                        if (ability.AbilityConfig.SHIELD)
                        {
                            ref var damageFactor2 = ref _world.GetPool<DamageFactorComponent>().Get(ownerEntity);
                            damageFactor2.ImpactList.Remove(ability.AbilityConfig.shieldDamagePhysicalFactor);
                        }
                    })
                    .AddTo(_disposable);
            }
            
            if (ability.AbilityConfig.DASH)
                _unitService.TryDash(ownerEntity, ability.AbilityConfig, false, Vector3.zero);
        }

        private void SelectAbility(int abilityEntity)
        {
            var ability = _world.GetPool<AbilityComponent>().Get(abilityEntity);
            if (ability.AbilityConfig.abilityType == EAbilityType.Skill)
                Attack(abilityEntity);
            else if (ability.AbilityConfig.abilityType == EAbilityType.Action &&
                     ability.AbilityConfig.actionType == EActionType.Outer)
                Action(abilityEntity);
        }

        private void Attack(int abilityEntity)
        {
            var ability = _world.GetPool<AbilityComponent>().Get(abilityEntity);

            if (!ability.Owner.Unpack(_world, out var ownerEntity))
                return;

            var owner = _unitService.GetUnit(ownerEntity);
            var vfxSpawnPoint = owner.View.VfxSpawnPoint;
            var spawnCenter = ability.AbilityConfig.vfxGameobject != null
                ? vfxSpawnPoint.position
                : owner.View.Transform.position;

            spawnCenter = spawnCenter +
                          vfxSpawnPoint.forward * ability.AbilityConfig.vfxPosition.z +
                          vfxSpawnPoint.right * ability.AbilityConfig.vfxPosition.x +
                          Vector3.up * ability.AbilityConfig.vfxPosition.y;

            float tempRadius;
            if (ability.AbilityConfig.useRadius)
                tempRadius = ability.AbilityConfig.radius;
            else tempRadius = 1000;

            var hitColliders = Physics.OverlapSphere(spawnCenter, tempRadius);
            foreach (var hit in hitColliders)
            {
                if (hit.transform.parent == null)
                    continue;
                if (hit.transform.parent.transform.parent == owner.View.Transform)
                    continue;
                if (_layersConfig.attackLayer.value != 1 << hit.gameObject.layer)
                    continue;

                var forward = owner.View.Transform.TransformDirection(Vector3.forward);
                var direction = (hit.transform.position - owner.View.Transform.position).normalized;
                var dotProduct = Vector3.Dot(forward, direction);

                float tempAngle;
                if (ability.AbilityConfig.useAngle)
                    tempAngle = ability.AbilityConfig.angle;
                else tempAngle = 1;

                var angle = tempAngle;
                var anglePercent = (180f - angle) / 180f;
                if (dotProduct >= anglePercent)
                {
                    var targetView = hit.transform.parent.transform.parent.GetComponent<UnitView>();
                    if (!targetView)
                        return;
                    if (!targetView.Entity.Unpack(_world, out var targetEntity))
                        return;
                    if (_unitService.HasState(targetEntity, EUnitState.Dead))
                        return;

                    TargetHitLogic(ability.AbilityConfig, targetView.Transform, targetEntity);
                }
            }
        }

        private void Action(int abilityEntity)
        {
            var ability = _world.GetPool<AbilityComponent>().Get(abilityEntity);

            if (!ability.Owner.Unpack(_world, out var ownerEntity))
                return;

            var owner = _unitService.GetUnit(ownerEntity);
            var center = ability.AbilityConfig.vfxGameobject != null
                ? owner.View.VfxSpawnPoint.position
                : owner.View.Transform.position;

            float tempRadius;
            if (ability.AbilityConfig.SHAPE && ability.AbilityConfig.useRadius)
                tempRadius = ability.AbilityConfig.radius;
            else
                tempRadius = 0;

            var hitColliders = Physics.OverlapSphere(center, tempRadius);

            foreach (var hit in hitColliders)
            {
                if (hit.CompareTag(ETags.door.ToString()))
                {
                    var doorView = hit.GetComponent<DoorView>();
                    _doorService.TryActivateDoor(doorView, ownerEntity);
                }
                else if (hit.CompareTag(ETags.item.ToString()))
                {
                    var itemView = hit.GetComponent<ItemView>();
                    if (!itemView.Entity.Unpack(_world, out var itemEntity))
                        return;

                    _itemService.ActivateItem(itemEntity);
                }
            }
        }

        private void TargetHitLogic(AbilityConfig abilityConfig, Transform ownerTransform, int targetEntity)
        {
            _unitService.AddDamage(targetEntity, abilityConfig.damage, ownerTransform);

            if (!_unitService.HasState(targetEntity, EUnitState.Dead) && abilityConfig.STUN)
            {
                _unitService.AddState(targetEntity, EUnitState.Stun);
                _unitService.AddState(targetEntity, EUnitState.Knockout);
            }
        }

        public bool IsEnabled(int entity) => _enabledPool.Has(entity);

        public void OnDestroy() => _disposable.Dispose();
    }
}