using CyberNinja.Services;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Item
{
    public class InitItemsSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<IItemService> _itemService;
        private readonly EcsCustomInject<SceneView> _sceneView;
        

        public void Init(IEcsSystems systems)
        {
            foreach (var view in _sceneView.Value.ItemContainer.Items)
                _itemService.Value.CreateItem(view);
        }
    }
}