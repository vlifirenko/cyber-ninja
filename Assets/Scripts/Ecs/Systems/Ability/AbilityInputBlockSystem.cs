using CyberNinja.Ecs.Components.Ability;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Ability
{
    public class AbilityInputBlockSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<AbilityInputBlockComponent>> _filter;
        private readonly EcsPoolInject<AbilityInputBlockComponent> _abilityInputBlockPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var inputBlock =ref _abilityInputBlockPool.Value.Get(entity);
                inputBlock.Value -= Time.deltaTime;

                if (inputBlock.Value <= 0)
                    _abilityInputBlockPool.Value.Del(entity);
            }
        }
    }
}