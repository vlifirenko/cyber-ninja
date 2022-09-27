using System.Linq;
using CyberNinja.Ecs.Components.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class DamageFactorSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DamageFactorComponent>> _filter;
        private readonly EcsPoolInject<DamageFactorComponent> _damageFactorPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var damageFactor =ref _damageFactorPool.Value.Get(entity);
                
                if (damageFactor.ImpactList.Count == 0)
                {
                    damageFactor.PhysicalFactor = 0;
                    continue;
                }

                var allDamageFactors = damageFactor.ImpactList.Sum();

                allDamageFactors = Mathf.Clamp(allDamageFactors, -100, 100);
                damageFactor.PhysicalFactor = allDamageFactors;
            }
        }
    }
}