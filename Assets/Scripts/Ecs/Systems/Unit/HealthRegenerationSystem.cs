using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class HealthRegenerationSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HealthRegenerationComponent>> _filter;
        private readonly EcsCustomInject<IUnitService> _unitService;
        private readonly EcsPoolInject<HealthRegenerationComponent> _healthRegenerationPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var health = _unitService.Value.GetHealth(entity);
                var healthRegeneration = _healthRegenerationPool.Value.Get(entity);

                if (health.Current > 0 && health.Current <= health.Max && healthRegeneration.Value != 0)
                {
                    var value = Mathf.Clamp(health.Current + healthRegeneration.Value * Time.deltaTime,
                        0, health.Max);
                    _unitService.Value.UpdateHealth(entity, value);
                }
            }
        }
    }
}