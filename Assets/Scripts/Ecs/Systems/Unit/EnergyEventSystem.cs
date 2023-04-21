using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Events;
using CyberNinja.Services;
using CyberNinja.Services.Unit;
using CyberNinja.Utils;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class EnergyEventSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EnergyComponent>> _filter;
        private readonly EcsCustomInject<UnitService> _unitService;
        private readonly EcsPoolInject<EnergyComponent> _energyPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var energy = ref _energyPool.Value.Get(entity);
                if (!energy.IsDirty)
                    return;

                energy.IsDirty = false;

                UnitEventsHolder.UpdateEnergy(entity, new Vector2(energy.Current, energy.Max));
            }
        }
    }
}