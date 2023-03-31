using CyberNinja.Events;
using CyberNinja.Services;
using CyberNinja.Services.Impl;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Item
{
    public class TryPickupItemService : IEcsInitSystem
    {
        private readonly EcsCustomInject<PlayerService> _unitService;
        private readonly EcsCustomInject<ItemService> _itemService;
        private readonly EcsWorldInject _world;

        public void Init(IEcsSystems systems)
        {
            ItemEventsHolder.OnTryPickup += OnTryPickup;
        }

        private void OnTryPickup(SceneObjectView view)
        {
            var config = view.Config;
            var itemEntity = _itemService.Value.CreateItem(config.item);
            var playerEntity = _unitService.Value.GetEntity();
            
            _itemService.Value.TryEquip(itemEntity, _world.Value.PackEntityWithWorld(playerEntity));
            view.Hide();
        }
    }
}