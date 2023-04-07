using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
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
        private EcsPoolInject<HealthComponent> _healthPool;
        private EcsPoolInject<UnitComponent> _unitPool;
        private EcsCustomInject<UnitService> _unitService;
        private EcsCustomInject<GlobalUnitConfig> _globalUnitConfig;
        private EcsCustomInject<VfxService> _vfxService;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var damage = _damagePool.Value.Get(entity);
                
                ref var health = ref _healthPool.Value.Get(entity);
                var unit = _unitPool.Value.Get(entity);
                //var damageFactor = _damageFactorPool.Get(entity);

                //var damageMath = damage - damage * damageFactor.PhysicalFactor / 100;
                var newHealth = Mathf.Clamp(health.Current - damage.Value.value, 0f, health.Max);

                _unitService.Value.UpdateHealth(entity, newHealth);

                var damageClamped = Mathf.Clamp01(damage.Value.value / _globalUnitConfig.Value.maxDamage);
                var healthClamped = Mathf.Clamp01(1 - health.Current / health.Max);

                var abilityData = unit.View.Config.AbilityDamageConfig;
                if (damage.Value.value > 0)
                {
                    if (abilityData.ANIMATOR)
                        unit.View.Animator.TriggerAnimations(abilityData);
                    if (abilityData.VFX)
                        _vfxService.Value.SpawnVfx(entity, abilityData, true, damageClamped, healthClamped,
                            damage.Value.damageOrigin.position);

                    var damageClampedLayer = Mathf.Clamp(damageClamped, _globalUnitConfig.Value.minLayerHit, 1); // limit min layer weight
                    unit.View.Animator.SetLayerWeight(2, damageClampedLayer);
                }

                //todo if (health.Current <= 0)
                //todo     Dead(entity);
                
                _damagePool.Value.Del(entity);
            }
        }
    }
}