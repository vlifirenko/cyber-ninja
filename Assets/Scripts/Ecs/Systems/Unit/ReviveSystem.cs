using System;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Enums;
using CyberNinja.Services;
using CyberNinja.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UniRx;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class ReviveSystem : IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsFilterInject<Inc<DeadComponent>> _filter;
        private readonly EcsCustomInject<IUnitService> _unitService;
        private readonly EcsCustomInject<IVfxService> _vfxService;
        private readonly EcsCustomInject<IAiService> _aiService;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var health = _unitService.Value.GetHealth(entity);
                if (health.Current > 0)
                    Revive(entity);
            }
        }

        private void Revive(int entity)
        {
            var unit = _unitService.Value.GetUnit(entity);
            var abilityData = unit.View.AbilityReviveConfig;

            _unitService.Value.RemoveState(entity, EUnitState.Dead);

            Observable.Timer(TimeSpan.FromSeconds(abilityData.duration))
                .Subscribe(_ =>
                {
                    if (_unitService.Value.HasState(entity, EUnitState.Dead))
                        return;
                    _unitService.Value.AddState(entity, EUnitState.Knockout);
                    if (unit.ControlType == EControlType.AI)
                        _aiService.Value.ReplaceAiTask(entity, EAiTaskType.Idle);
                })
                .AddTo(_disposable);

            unit.View.NavMeshAgent.enabled = true;

            if (abilityData.ANIMATOR)
                unit.View.Animator.TriggerAnimations(abilityData);
            if (abilityData.VFX)
                _vfxService.Value.SpawnVfx(entity, abilityData);
        }

        public void Destroy(IEcsSystems systems) => _disposable.Dispose();
    }
}