using CyberNinja.Ecs.Components.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class StunSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<StunComponent>> _filter;
        private readonly EcsPoolInject<StunComponent> _pool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var stun = ref _pool.Value.Get(entity);
                stun.TimeLeft -= Time.deltaTime;

                if (stun.TimeLeft <= 0) 
                    _pool.Value.Del(entity);
            }
        }
    }
}