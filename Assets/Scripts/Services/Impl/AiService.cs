using System;
using CyberNinja.Ecs.Components;
using CyberNinja.Ecs.Components.Ability;
using CyberNinja.Ecs.Components.Ai;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Enums;
using Leopotam.EcsLite;
using UniRx;

namespace CyberNinja.Services.Impl
{
    public class AiService : IAiService
    {
        private readonly EcsWorld _world;
        private readonly AbilityService _abilityService;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly EcsPool<AiTaskComponent> _aiTaskPool;

        public AiService(EcsWorld world, AbilityService abilityService)
        {
            _world = world;
            _abilityService = abilityService;

            _aiTaskPool = _world.GetPool<AiTaskComponent>();
        }

        public void InitUnit(int entity)
        {
            _world.GetPool<AiTaskComponent>().Add(entity).Value = EAiTaskType.Idle;
        }

        public void ReplaceAiTask(int entity, EAiTaskType task)
        {
            ref var aiTask = ref _aiTaskPool.Get(entity);
            aiTask.Value = task;
        }

        public void TryAttack(int unitEntity)
        {
            var aiTaskPool = _world.GetPool<AiTaskComponent>();
            var deadPool = _world.GetPool<DeadComponent>();

            if (deadPool.Has(unitEntity) || !aiTaskPool.Has(unitEntity))
                return;

            ref var aiTask = ref _world.GetPool<AiTaskComponent>().Get(unitEntity);
            if (aiTask.Value == EAiTaskType.Attack)
                return;
            
            aiTask.Value = EAiTaskType.Attack;
            Attack(unitEntity);
        }

        private void Attack(int unitEntity)
        {
            var aiTask = _world.GetPool<AiTaskComponent>().Get(unitEntity);
            if (aiTask.Value != EAiTaskType.Attack)
                return;

            var (success, abilityEntity) = _abilityService.TryActivateAbility(0, unitEntity);
            if (!success)
                return;

            if (!_world.GetPool<AbilityComponent>().Has(abilityEntity))
                return;
            
            var ability = _world.GetPool<AbilityComponent>().Get(abilityEntity);
            Observable.Timer(TimeSpan.FromSeconds(ability.AbilityConfig.cooldown + 0.01f))
                .Subscribe(_ => Attack(unitEntity))
                .AddTo(_disposable);
        }

        public void OnDestroy() => _disposable.Dispose();
    }
}