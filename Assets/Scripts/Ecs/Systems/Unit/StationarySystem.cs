using CyberNinja.Ecs.Components.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class StationarySystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<StationaryComponent>> _filter;
        private readonly EcsPoolInject<StationaryComponent> _pool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var stationary = ref _pool.Value.Get(entity);

                stationary.Time -= Time.deltaTime;

                if (stationary.Time <= 0)
                    _pool.Value.Del(entity);
            }
        }
    }
}