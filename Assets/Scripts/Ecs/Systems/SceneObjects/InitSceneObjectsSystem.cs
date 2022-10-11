using CyberNinja.Services;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.SceneObjects
{
    public class InitSceneObjectsSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<ISceneService> _sceneService;
        private readonly EcsCustomInject<SceneView> _sceneView;

        public void Init(IEcsSystems systems)
        {
            foreach (var view in _sceneView.Value.SceneObjectContainer.Items)
                _sceneService.Value.CreateObject(view);
        }
    }
}