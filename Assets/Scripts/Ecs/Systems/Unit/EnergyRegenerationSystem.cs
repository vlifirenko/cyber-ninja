using CyberNinja.Ecs.Components.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class EnergyRegenerationSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EnergyRegenerationComponent>> _filter;
        private readonly EcsPoolInject<EnergyComponent> _energyPool;
        private readonly EcsPoolInject<EnergyRegenerationComponent> _energyRegenerationPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var energy = ref _energyPool.Value.Get(entity);
                var energyRegeneration = _energyRegenerationPool.Value.Get(entity);

                if (energy.Current > 0 && energy.Current <= energy.Max && energyRegeneration.Value != 0)
                {
                    var value = Mathf.Clamp(energy.Current + energyRegeneration.Value * Time.deltaTime,
                        0, energy.Max);
                    energy.Current = value;
                }
            }
        }
    }
}