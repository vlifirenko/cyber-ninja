using CyberNinja.Ecs.Components.Debug;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class PushSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<PushComponent>, Exc<DeadComponent, KnockoutComponent>> _filter;
        private EcsFilterInject<Inc<DebugSelectedComponent>> _debugSelectedFilter;
        private EcsPoolInject<PushComponent> _pushPool;
        private EcsPoolInject<UnitComponent> _unitPool;
        private EcsWorldInject _world;
        private EcsCustomInject<GameData> _gameData;
        private EcsCustomInject<GlobalUnitConfig> _globalUnitConfig;

        public void Init(IEcsSystems systems)
        {
            _gameData.Value.Input.Debug.Enable();
            _gameData.Value.Input.Debug.Push.performed += ctg =>
            {
                foreach (var entity in _debugSelectedFilter.Value)
                {
                    // push
                    if (!_world.Value.GetPool<PushComponent>().Has(entity))
                    {
                        var unit = _unitPool.Value.Get(entity);
                        var direction = -unit.View.Transform.forward;
                        _world.Value.GetPool<PushComponent>().Add(entity) = new PushComponent
                        {
                            Directon = direction.normalized,
                            CurrentTime = 0f,
                            TargetTime = _globalUnitConfig.Value.pushLength,
                            Speed = _globalUnitConfig.Value.pushSpeed
                        };
                    }
                }
            };
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var push = ref _pushPool.Value.Get(entity);
                var unit = _unitPool.Value.Get(entity);

                push.CurrentTime += Time.deltaTime;

                var speed = _world.Value.GetPool<SpeedComponent>().Get(entity);
                var targetTranslation = push.Directon * Time.deltaTime * push.Speed;

                unit.View.NavMeshAgent.Move(targetTranslation);

                if (push.CurrentTime >= push.TargetTime)
                    _pushPool.Value.Del(entity);

                //var targetDebugLine = unit.View.Transform.position + targetTranslation;
                //Debug.DrawLine(unit.View.Transform.position, targetDebugLine);
            }
        }
    }
}