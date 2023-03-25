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
    public class AiChaseSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<AiTaskComponent, AiTargetComponent>, Exc<KnockoutComponent, FreezeComponent>> _filter;
        private readonly EcsCustomInject<IAiService> _aiService;
        private readonly EcsCustomInject<IUnitService> _unitService;
        private readonly EcsPoolInject<AiTaskComponent> _aiTaskPool;
        private readonly EcsPoolInject<AiTargetComponent> _aiTargetPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var aiTask = ref _aiTaskPool.Value.Get(entity);
                if (aiTask.Value != EAiTaskType.Chase)
                    continue;

                var unit = _unitService.Value.GetUnit(entity);
                ref var aiTarget = ref _aiTargetPool.Value.Get(entity);

                aiTarget.Distance = Vector3.Distance(unit.View.Transform.position, aiTarget.Transform.position);

                if (aiTarget.Distance > unit.Config.MaxChaseDistance)
                {
                    aiTask.Value = EAiTaskType.Idle;
                    _aiTargetPool.Value.Del(entity);
                }
                else if (aiTarget.Distance <= unit.Config.AttackDistance)
                {
                    _aiService.Value.TryAttack(entity);
                }
            }
        }
    }
}