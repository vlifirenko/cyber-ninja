using CyberNinja.Ecs.Components.Ability;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Ability
{
    public class AbilityCooldownSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<AbilityCooldownComponent>> _filter;
        private readonly EcsPoolInject<AbilityCooldownComponent> _abilityCooldownPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var abilityCooldown = ref _abilityCooldownPool.Value.Get(entity);

                if (abilityCooldown.Value > 0)
                    abilityCooldown.Value -= Time.deltaTime;
                else
                    _abilityCooldownPool.Value.Del(entity);
            }
        }
    }
}