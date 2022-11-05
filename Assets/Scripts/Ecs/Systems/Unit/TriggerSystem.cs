using System.Collections.Generic;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Services;
using CyberNinja.Services.Unit;
using CyberNinja.Utils;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class TriggerSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<OnTriggerEnterEvent>> _triggerEnterFilter;
        private readonly EcsFilterInject<Inc<OnTriggerExitEvent>> _triggerExitFilter;
        private readonly EcsPoolInject<OnTriggerEnterEvent> _triggerEnterPool;
        private readonly EcsPoolInject<OnTriggerExitEvent> _triggerExitPool;
        private readonly EcsPoolInject<TriggerComponent> _triggerPool;
        private readonly EcsCustomInject<ISceneService> _sceneService;
        private readonly EcsCustomInject<IUnitService> _unitService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _triggerEnterFilter.Value)
            {
                var eventData = _triggerEnterPool.Value.Get(entity);
                if (eventData.collider.CompareTag(Tag.Item))
                    OnTriggerEnter(_unitService.Value.GetPlayerEntity(), eventData.collider.transform);
            }

            foreach (var entity in _triggerExitFilter.Value)
            {
                var eventData = _triggerExitPool.Value.Get(entity);
                if (eventData.collider.CompareTag(Tag.Item))
                    OnTriggerExit(_unitService.Value.GetPlayerEntity(), eventData.collider.transform);
            }
        }

        private void OnTriggerEnter(int entity, Transform transform)
        {
            if (!_triggerPool.Value.Has(entity))
                _triggerPool.Value.Add(entity).Transforms = new List<Transform> {transform};
            else
            {
                ref var triggerEntity = ref _triggerPool.Value.Get(entity);
                triggerEntity.Transforms.Add(transform);
            }
        }

        private void OnTriggerExit(in int entity, Transform transform)
        {
            ref var trigger = ref _triggerPool.Value.Get(entity);
            trigger.Transforms.Remove(transform);
            if (trigger.Transforms.Count == 0)
                _triggerPool.Value.Del(entity);
        }
    }
}