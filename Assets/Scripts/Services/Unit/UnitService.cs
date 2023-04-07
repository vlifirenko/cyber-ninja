using System;
using System.Collections.Generic;
using CyberNinja.Ecs.Components.Ai;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Models.Enums;
using CyberNinja.Services.Impl;
using CyberNinja.Utils;
using CyberNinja.Views;
using CyberNinja.Views.Core;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using UnityEngine;

namespace CyberNinja.Services.Unit
{
    public class UnitService
    {
        private readonly EcsWorld _world;
        private readonly GlobalUnitConfig _globalUnitConfig;
        private readonly IVfxService _vfxService;
        private readonly ItemService _itemService;
        private readonly EcsPool<StunComponent> _stunPool;
        private readonly EcsPool<KnockoutComponent> _knockoutPool;
        private readonly EcsPool<DeadComponent> _deadPool;
        private readonly EcsPool<HealthComponent> _healthPool;
        private readonly EcsPool<UnitComponent> _unitPool;
        private readonly EcsPool<DamageFactorComponent> _damageFactorPool;
        private readonly EcsPool<DashComponent> _dashPool;
        private readonly EcsPool<StationaryComponent> _stationaryPool;
        private readonly EcsPool<PlayerComponent> _playerPool;
        private readonly EcsPool<EnergyComponent> _energyPool;
        private readonly EcsPool<VectorsComponent> _vectorsPool;
        private readonly EcsPool<MoveVectorComponent> _moveVectorPool;
        private readonly EcsPool<TriggerComponent> _triggerPool;
        private CanvasView _canvasView;

        private EcsPackedEntity _playerEntity;

        public UnitService(EcsWorld world, GlobalUnitConfig globalUnitConfig, CanvasView canvasView, IVfxService vfxService,
            ItemService itemService)
        {
            _world = world;
            _globalUnitConfig = globalUnitConfig;
            _vfxService = vfxService;
            _itemService = itemService;
            _canvasView = canvasView;

            _stunPool = _world.GetPool<StunComponent>();
            _knockoutPool = _world.GetPool<KnockoutComponent>();
            _deadPool = _world.GetPool<DeadComponent>();
            _healthPool = _world.GetPool<HealthComponent>();
            _unitPool = _world.GetPool<UnitComponent>();
            _damageFactorPool = _world.GetPool<DamageFactorComponent>();
            _dashPool = _world.GetPool<DashComponent>();
            _stationaryPool = _world.GetPool<StationaryComponent>();
            _playerPool = _world.GetPool<PlayerComponent>();
            _energyPool = _world.GetPool<EnergyComponent>();
            _vectorsPool = _world.GetPool<VectorsComponent>();
            _moveVectorPool = _world.GetPool<MoveVectorComponent>();
            _triggerPool = _world.GetPool<TriggerComponent>();
        }

        public IAbilityService AbilityService { get; set; }

        public int CreateUnit(UnitView view)
        {
            var entity = _world.NewEntity();

            ref var unit = ref _unitPool.Add(entity);
            unit.View = view;
            unit.Config = view.Config;

            ref var health = ref _healthPool.Add(entity);
            health.Current = view.Config.MaxHealth;
            health.Max = view.Config.MaxHealth;

            var healthRegenerationPool = _world.GetPool<HealthRegenerationComponent>();
            ref var healthRegeneration = ref healthRegenerationPool.Add(entity);
            healthRegeneration.Value = view.Config.HealthRegeneration;

            var energyPool = _world.GetPool<EnergyComponent>();
            ref var energy = ref energyPool.Add(entity);
            energy.Current = view.Config.MaxEnergy;
            energy.Max = view.Config.MaxEnergy;

            var energyRegenerationPool = _world.GetPool<EnergyRegenerationComponent>();
            ref var energyRegeneration = ref energyRegenerationPool.Add(entity);
            energyRegeneration.Value = view.Config.EnergyRegeneration;

            var damageFactorPool = _world.GetPool<DamageFactorComponent>();
            ref var damageFactor = ref damageFactorPool.Add(entity);
            damageFactor.ImpactList = new List<float>();
            damageFactor.PhysicalFactor = 0;

            var movementPool = _world.GetPool<SpeedComponent>();
            ref var movement = ref movementPool.Add(entity);
            movement.SpeedMoveMax = view.Config.MoveSpeed;
            movement.SpeedCurrent = 0;
            movement.SpeedTarget = 0;

            var vectorsPool = _world.GetPool<VectorsComponent>();
            ref var vectors = ref vectorsPool.Add(entity);
            vectors.IsActiveVectorLook = true;
            vectors.VectorLook = Vector3.zero;
            vectors.VectorDifference = Vector3.zero;

            view.NavMeshAgent.speed = movement.SpeedCurrent;
            view.NavMeshAgent.stoppingDistance = view.Config.AttackDistance - 0.1f;
            view.NavMeshAgent.updateRotation = false;

            if (unit.Config.DefaultWeapon != null)
            {
                var weaponEntity = _itemService.CreateItem(unit.Config.DefaultWeapon);
                _itemService.TryEquip(weaponEntity, _world.PackEntityWithWorld(entity));

                var ability = unit.Config.Abilities[0];
                AbilityService.CreateAbility(ability, entity);
                _canvasView.AbilityImages[ability.slotIndex].sprite = ability.abilityConfig.icon;
            }

            if (view.IsFreeze)
                _world.GetPool<FreezeComponent>().Add(entity);

            view.Entity = _world.PackEntity(entity);

            _world.GetPool<TargetsComponent>().Add(entity).Targets = new List<Target>();

            return entity;
        }

        public void AddDamage(int entity, float damage, Transform damageOrigin)
        {
            var damageFactor = _damageFactorPool.Get(entity);
            // todo move to system
            /*_world.GetPool<DamageComponent>().Add(entity) = new DamageComponent
            {
                Value = new Damage
                {
                    value = damageFactor.PhysicalFactor,
                    damageOrigin = damageOrigin
                }
            };*/
            //
            ref var health = ref _healthPool.Get(entity);
            var unit = _unitPool.Get(entity);

            var damageMath = damage - damage * damageFactor.PhysicalFactor / 100;
            var newHealth = Mathf.Clamp(health.Current - damageMath, 0f, health.Max);

            UpdateHealth(entity, newHealth);

            var damageClamped = Mathf.Clamp01(damageMath / _globalUnitConfig.maxDamage);
            var healthClamped = Mathf.Clamp01(1 - health.Current / health.Max);

            var abilityData = unit.View.Config.AbilityDamageConfig;
            if (damageMath > 0)
            {
                if (abilityData.ANIMATOR)
                    unit.View.Animator.TriggerAnimations(abilityData);
                if (abilityData.VFX)
                    _vfxService.SpawnVfx(entity, abilityData, true, damageClamped, healthClamped, damageOrigin.position);

                var damageClampedLayer = Mathf.Clamp(damageClamped, _globalUnitConfig.minLayerHit, 1); // limit min layer weight
                unit.View.Animator.SetLayerWeight(2, damageClampedLayer);
            }

            if (health.Current <= 0)
                Dead(entity);
        }

        private void Dead(int entity)
        {
            // todo move to system
            
            AddState(entity, EUnitState.Dead);
            AddState(entity, EUnitState.Knockout);

            RemoveState(entity, EUnitState.Stun);
            RemoveState(entity, EUnitState.Dash);
            RemoveState(entity, EUnitState.Stationary);

            var unit = _unitPool.Get(entity);

            unit.View.NavMeshAgent.enabled = false;

            if (unit.View.Config.AbilityDeadConfig.ANIMATOR)
                unit.View.Animator.TriggerAnimations(unit.View.Config.AbilityDeadConfig);
            if (unit.View.Config.AbilityDeadConfig.VFX)
                _vfxService.SpawnVfx(entity, unit.View.Config.AbilityDeadConfig);

            if (unit.Config.ControlType == EControlType.AI)
            {
                ref var aiTask = ref _world.GetPool<AiTaskComponent>().Get(entity);
                aiTask.Value = EAiTaskType.Dead;
                if (_world.GetPool<AiTargetComponent>().Has(entity))
                    _world.GetPool<AiTargetComponent>().Del(entity);

                ref var enemy = ref _world.GetPool<EnemyComponent>().Get(entity);
                enemy.HealthSlider.gameObject.SetActive(false);
            }
        }

        public bool AddState(int entity, EUnitState state, float value = 0)
        {
            switch (state)
            {
                case EUnitState.Stun:
                    if (_stunPool.Has(entity))
                        return false;
                    _stunPool.Add(entity);
                    ToggleStun(entity);
                    return true;
                case EUnitState.Knockout:
                    if (_knockoutPool.Has(entity))
                        return false;
                    _knockoutPool.Add(entity);
                    return true;
                case EUnitState.Dead:
                    if (_deadPool.Has(entity))
                        return false;
                    _deadPool.Add(entity);
                    return true;
                case EUnitState.Dash:
                    if (_dashPool.Has(entity))
                        return false;
                    _dashPool.Add(entity);
                    return true;
                case EUnitState.Stationary:
                    if (_stationaryPool.Has(entity))
                        return false;
                    _stationaryPool.Add(entity).Time = value;
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public bool HasState(int entity, EUnitState state)
        {
            switch (state)
            {
                case EUnitState.Stun:
                    return _stunPool.Has(entity);
                case EUnitState.Knockout:
                    return _knockoutPool.Has(entity);
                case EUnitState.Dead:
                    return _deadPool.Has(entity);
                case EUnitState.Dash:
                    return _dashPool.Has(entity);
                case EUnitState.Stationary:
                    return _stationaryPool.Has(entity);
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public bool RemoveState(int entity, EUnitState state)
        {
            switch (state)
            {
                case EUnitState.Stun:
                    if (!_stunPool.Has(entity))
                        return false;
                    _stunPool.Del(entity);
                    ToggleStun(entity);
                    return true;
                case EUnitState.Knockout:
                    if (!_knockoutPool.Has(entity))
                        return false;
                    _knockoutPool.Del(entity);
                    return true;
                case EUnitState.Dead:
                    if (!_deadPool.Has(entity))
                        return false;
                    _deadPool.Del(entity);
                    return true;
                case EUnitState.Dash:
                    if (!_dashPool.Has(entity))
                        return false;
                    _dashPool.Del(entity);
                    return true;
                case EUnitState.Stationary:
                    if (!_stationaryPool.Has(entity))
                        return false;
                    _stationaryPool.Del(entity);
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public bool IsPlayer(int entity) => _playerPool.Has(entity);
        
        public EcsPackedEntity Player { get; set; }

        public HealthComponent GetHealth(int entity)
        {
            if (_healthPool.Has(entity))
                return _healthPool.Get(entity);
            throw new Exception("Entity hasn't health component");
        }

        public void UpdateHealth(int entity, float value)
        {
            ref var health = ref _healthPool.Get(entity);

            health.Current = value;
            health.IsDirty = true;

            if (IsPlayer(entity))
            {
            }
        }

        public void UpdateEnergy(int entity, float value)
        {
            ref var energy = ref _energyPool.Get(entity);

            energy.Current = value;
            energy.IsDirty = true;
        }

        public UnitComponent GetUnit(int entity)
        {
            if (_unitPool.Has(entity))
                return _unitPool.Get(entity);
            throw new Exception("Entity hasn't unit component");
        }

        public void TryDash(int entity, AbilityConfig abilityConfig, bool hit, Vector3 hitVector)
        {
            if (abilityConfig.DASH)
            {
                var time = abilityConfig.dashTime;
                var distance = abilityConfig.dashDistance;
                var useLook = abilityConfig.dashUseLook;

                if (HasState(entity, EUnitState.Dash))
                {
                    ref var dash = ref _dashPool.Get(entity);
                    if (time > dash.TimeLeft)
                    {
                        dash.Time = time;
                        dash.TimeLeft = time;
                        dash.Distance = distance;
                        dash.UseLook = useLook;
                    }
                }
                else
                {
                    Vector3 vector;
                    if (!hit)
                    {
                        var vectors = _vectorsPool.Get(entity);
                        var moveVector = _moveVectorPool.Get(entity);
                        var unit = GetUnit(entity);

                        if (abilityConfig.dashUseLook)
                            vector = vectors.VectorLook;
                        else
                            vector = moveVector.Value;

                        if (vector == Vector3.zero)
                            vector = unit.View.Transform.forward;
                    }
                    else
                    {
                        vector = hitVector;
                        Debug.Log("bam");
                    }

                    _dashPool.Add(entity) = new DashComponent
                    {
                        Time = time,
                        TimeLeft = time,
                        Distance = distance,
                        Vector = vector,
                        UseLook = useLook,
                        VectorActivated = true
                    };
                }
            }
        }

        public AView GetTrigger(int entity)
        {
            var trigger = _triggerPool.Get(entity);
            if (trigger.Transforms.Count == 0)
                return null;

            var view = trigger.Transforms[0].GetComponent<AView>();
            if (view == null)
                return null;

            return view;
        }
        
        private void ToggleStun(int entity)
        {
            var unit = _unitPool.Get(entity);
            var abilityData = unit.View.Config.AbilityDamageConfig;

            if (unit.View.Config.AbilityStunOnConfig.ANIMATOR)
            {
                if (HasState(entity, EUnitState.Stun))
                {
                    AddState(entity, EUnitState.Knockout);
                    unit.View.Animator.TriggerAnimations(unit.View.Config.AbilityStunOnConfig);
                }
                else
                {
                    RemoveState(entity, EUnitState.Knockout);
                    unit.View.Animator.TriggerAnimations(unit.View.Config.AbilityStunOffConfig);
                }
            }


            if (abilityData.VFX)
                _vfxService.SpawnVfx(entity, abilityData);
        }
    }
}