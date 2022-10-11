using CyberNinja.Services;
using CyberNinja.Utils;
using CyberNinja.Views;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class TriggerEnterSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<OnTriggerEnterEvent>> _filter;
        private readonly EcsPoolInject<OnTriggerEnterEvent> _triggerPool;
        private readonly EcsCustomInject<ISceneService> _sceneService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var eventData = ref _triggerPool.Value.Get(entity);

                if (eventData.collider.CompareTag(Tag.Item))
                {
                    if (eventData.collider.TryGetComponent<SceneObjectView>(out var view))
                        _sceneService.Value.TriggerObject(view);
                }
            }
        }
    }
}