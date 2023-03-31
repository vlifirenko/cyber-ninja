using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Player
{
    public class DroidMovementSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<DroidComponent>, Exc<DeadComponent>> _filter;
        private EcsWorldInject _world;
        private EcsCustomInject<GlobalUnitConfig> _globalUnitConfig;
        
        private float _timer;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var droid = _world.Value.GetPool<DroidComponent>().Get(entity);
                var unit = _world.Value.GetPool<UnitComponent>().Get(entity);
                var distance = Vector3.Distance(droid.DroidView.Transform.position, unit.View.Transform.position);

                if (distance > _globalUnitConfig.Value.droidMoveDistance)
                {
                    var destination = unit.View.Transform.position;
                    destination.y = _globalUnitConfig.Value.droidYSpawnPosition;
                    
                    var position = Vector3.Lerp(
                        droid.DroidView.Transform.position,
                        destination,
                        _timer);

                    droid.DroidView.Transform.position = position;
                    _timer += Time.deltaTime * _globalUnitConfig.Value.droidSpeed;
                }
                else
                {
                    _timer = 0f;
                }
            }
        }
    }
}