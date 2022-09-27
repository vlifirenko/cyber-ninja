using CyberNinja.Ecs.Components.Ai;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Enums;
using CyberNinja.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Ai
{
    public class AiUpdateStateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<AiTaskComponent>> _filter;
        private readonly EcsCustomInject<IUnitService> _unitService;
        private readonly EcsPoolInject<SpeedComponent> _speedPool;
        private readonly EcsPoolInject<DeadComponent> _deadPool;
        private readonly EcsPoolInject<AiTaskComponent> _aiTaskPool;
        private readonly EcsPoolInject<StunComponent> _stunPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var view = _unitService.Value.GetUnit(entity).View;
                var speed = _speedPool.Value.Get(entity);
                ref var aiTask = ref _aiTaskPool.Value.Get(entity);

                view.NavMeshAgent.speed = speed.SpeedCurrent;

                if (_deadPool.Value.Has(entity))
                    aiTask.Value = EAiTaskType.Dead;
                else if (_stunPool.Value.Has(entity))
                    aiTask.Value = EAiTaskType.Stun;
            }
        }
    }
}