using CyberNinja.Ecs.Components.Item;
using CyberNinja.Views;
using Leopotam.EcsLite;
using UnityEngine;

namespace CyberNinja.Services.Impl
{
    public class ItemService : IItemService
    {
        private readonly EcsWorld _world;
        private readonly EcsPool<ItemComponent> _itemPool;

        public ItemService(EcsWorld world)
        {
            _world = world;
            _itemPool = _world.GetPool<ItemComponent>();
        }

        public int CreateItem(ItemView view)
        {
            var entity = _world.NewEntity();

            _itemPool.Add(entity);

            return entity;
        }

        public void ActivateItem(int entity)
        {
        }

        public void PickupItem(ItemView view)
        {
            Debug.Log($"Pickup {view.name}");
            view.Hide();
        }
    }
}