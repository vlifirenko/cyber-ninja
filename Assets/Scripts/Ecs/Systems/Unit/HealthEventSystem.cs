using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Events;
using CyberNinja.Services.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class HealthEventSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HealthComponent>> _filter;
        private readonly EcsCustomInject<IUnitService> _unitService;
        private readonly EcsPoolInject<HealthComponent> _healthPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var health = ref _healthPool.Value.Get(entity);
                if (!health.IsDirty)
                    return;

                health.IsDirty = false;
                
                UnitEventsHolder.UpdateHealth(entity, new Vector2(health.Current, health.Max));
            }
        }
    }
}