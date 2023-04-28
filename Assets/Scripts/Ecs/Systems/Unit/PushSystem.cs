using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class PushSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PushComponent>, Exc<DeadComponent, KnockoutComponent>> _filter;
        private EcsPoolInject<PushComponent> _pushPool;
        private EcsPoolInject<UnitComponent> _unitPool;
        private EcsWorldInject _world;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var push = ref _pushPool.Value.Get(entity);
                var unit = _unitPool.Value.Get(entity);

                push.CurrentTime += Time.deltaTime;
                
                var speed = _world.Value.GetPool<SpeedComponent>().Get(entity);
                var targetTranslation = push.Directon * speed.SpeedCurrent * Time.deltaTime * push.Speed;

                unit.View.NavMeshAgent.Move(targetTranslation);
                
                if (push.CurrentTime >= push.TargetTime)
                    _pushPool.Value.Del(entity);
            }
        }
    }
}