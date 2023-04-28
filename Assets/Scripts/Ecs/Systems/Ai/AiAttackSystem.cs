using CyberNinja.Ecs.Components.Ai;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Enums;
using CyberNinja.Services;
using CyberNinja.Services.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Ai
{
    public class AiAttackSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<
            Inc<AiTaskComponent, AiTargetComponent>,
            Exc<KnockoutComponent, FreezeComponent, PushComponent>> _filter;
        private readonly EcsPoolInject<AiTaskComponent> _aiTaskPool;
        private readonly EcsPoolInject<AiTargetComponent> _aiTargetPool;
        private readonly EcsCustomInject<UnitService> _unitService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var aiTask = ref _aiTaskPool.Value.Get(entity);
                if (aiTask.Value != EAiTaskType.Attack)
                    continue;

                var unit = _unitService.Value.GetUnit(entity);
                var aiTarget = _aiTargetPool.Value.Get(entity);
                if (!aiTarget.Entity.Unpack(_aiTaskPool.Value.GetWorld(), out var targetEntity))
                    return;

                if (_unitService.Value.HasState(targetEntity, EUnitState.Dead))
                    OnTargetDead(entity);

                var distance = Vector3.Distance(unit.View.Transform.position, aiTarget.Transform.position);

                if (distance >= unit.Config.MaxAttackDistance)
                    aiTask.Value = EAiTaskType.Chase;
            }
        }

        private void OnTargetDead(int entity)
        {
            ref var aiTask = ref _aiTaskPool.Value.Get(entity);
            aiTask.Value = EAiTaskType.Idle;
            _aiTargetPool.Value.Del(entity);
        }
    }
}