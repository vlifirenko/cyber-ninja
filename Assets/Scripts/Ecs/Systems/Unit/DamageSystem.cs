using CyberNinja.Ecs.Components.Ai;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Events;
using CyberNinja.Models.Config;
using CyberNinja.Models.Enums;
using CyberNinja.Services.Impl;
using CyberNinja.Services.Unit;
using CyberNinja.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class DamageSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<DamageComponent>> _filter;
        private EcsPoolInject<DamageComponent> _damagePool;
        private EcsWorldInject _world;
        private EcsCustomInject<UnitService> _unitService;
        private EcsCustomInject<GlobalUnitConfig> _globalUnitConfig;
        private EcsCustomInject<VfxService> _vfxService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var damage = _damagePool.Value.Get(entity).Value;

                ref var health = ref _world.Value.GetPool<HealthComponent>().Get(entity);
                var unit = _world.Value.GetPool<UnitComponent>().Get(entity);
                var newHealth = Mathf.Clamp(health.Current - damage.value, 0f, health.Max);

                _unitService.Value.UpdateHealth(entity, newHealth);

                var damageClamped = Mathf.Clamp01(damage.value / _globalUnitConfig.Value.maxDamage);
                var healthClamped = Mathf.Clamp01(1 - health.Current / health.Max);

                var abilityData = unit.View.Config.AbilityDamageConfig;
                if (damage.value > 0)
                {
                    if (abilityData.ANIMATOR)
                        unit.View.Animator.TriggerAnimations(abilityData);
                    if (abilityData.VFX)
                        _vfxService.Value.SpawnVfx(entity, abilityData, true, damageClamped, healthClamped,
                            damage.damageOrigin.position);

                    var damageClampedLayer =
                        Mathf.Clamp(damageClamped, _globalUnitConfig.Value.minLayerHit, 1); // limit min layer weight
                    unit.View.Animator.SetLayerWeight(2, damageClampedLayer);

                    // push
                    if (!_world.Value.GetPool<PushComponent>().Has(entity))
                    {
                        var direction = unit.View.Transform.position - damage.attacker.position;
                        _world.Value.GetPool<PushComponent>().Add(entity) = new PushComponent
                        {
                            Directon = direction.normalized,
                            CurrentTime = 0f,
                            TargetTime = _globalUnitConfig.Value.pushLength,
                            Speed = _globalUnitConfig.Value.pushSpeed
                        };
                    }
                }

                if (health.Current <= 0)
                    Dead(entity);

                _damagePool.Value.Del(entity);
            }
        }

        private void Dead(int entity)
        {
            _unitService.Value.AddState(entity, EUnitState.Dead);
            _unitService.Value.AddState(entity, EUnitState.Knockout);

            _unitService.Value.RemoveState(entity, EUnitState.Stun);
            _unitService.Value.RemoveState(entity, EUnitState.Dash);
            _unitService.Value.RemoveState(entity, EUnitState.Stationary);

            var unit = _world.Value.GetPool<UnitComponent>().Get(entity);

            unit.View.NavMeshAgent.enabled = false;

            if (unit.View.Config.AbilityDeadConfig.ANIMATOR)
                unit.View.Animator.TriggerAnimations(unit.View.Config.AbilityDeadConfig);
            if (unit.View.Config.AbilityDeadConfig.VFX)
                _vfxService.Value.SpawnVfx(entity, unit.View.Config.AbilityDeadConfig);

            if (unit.Config.ControlType == EControlType.AI)
            {
                ref var aiTask = ref _world.Value.GetPool<AiTaskComponent>().Get(entity);
                aiTask.Value = EAiTaskType.Dead;
                if (_world.Value.GetPool<AiTargetComponent>().Has(entity))
                    _world.Value.GetPool<AiTargetComponent>().Del(entity);

                ref var enemy = ref _world.Value.GetPool<EnemyComponent>().Get(entity);
                enemy.HealthSlider.gameObject.SetActive(false);
                
                SpawnLoot(entity);
            }

            EnemyEventsHolder.InvokeOnKillEnemy(entity);
        }

        private void SpawnLoot(int entity) => _world.Value.GetPool<SpawnLootComponent>().Add(entity);
    }
}