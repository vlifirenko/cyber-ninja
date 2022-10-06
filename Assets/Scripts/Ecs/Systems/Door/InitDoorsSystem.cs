using CyberNinja.Services;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Door
{
    public class InitDoorsSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<IDoorService> _doorService;
        private readonly EcsCustomInject<SceneView> _sceneView;

        public void Init(IEcsSystems systems)
        {
            foreach (var view in _sceneView.Value.DoorContainer.Items)
                _doorService.Value.CreateDoor(view);
        }
    }
}