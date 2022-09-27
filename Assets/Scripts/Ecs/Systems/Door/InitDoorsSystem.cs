using CyberNinja.Services;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Door
{
    public class InitDoorsSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<IDoorService> _doorService;
        private readonly EcsCustomInject<SceneView> _sceneView;
        
        public void Run(IEcsSystems systems)
        {
            var doors = _sceneView.Value.DoorContainerView.Items;
            if (doors.Length == 0)
                return;

            foreach (var door in doors)
            {
                var view = door.GetComponent<DoorView>();
                _doorService.Value.CreateDoor(view);
            }
        }
    }
}