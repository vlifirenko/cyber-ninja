using CyberNinja.Event;
using CyberNinja.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Game
{
    public class TimeSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<ITimeService> _timeService;

        public void Run(IEcsSystems systems)
        {
            _timeService.Value.Time = Time.time;
            _timeService.Value.UnscaledTime = Time.unscaledTime;
            _timeService.Value.DeltaTime = Time.deltaTime;
            _timeService.Value.UnscaledDeltaTime = Time.unscaledDeltaTime;
            
            EventsHolder.UpdateTime(_timeService.Value.Time);
        }
    }
}