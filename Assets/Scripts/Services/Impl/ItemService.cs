using System.Collections.Generic;
using CyberNinja.Ecs.Components.Item;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
using CyberNinja.Models.Enums;
using Leopotam.EcsLite;
using UnityEngine;

namespace CyberNinja.Services.Impl
{
    public class ItemService : IItemService
    {
        private readonly EcsWorld _world;
        private readonly EcsPool<ItemComponent> _itemPool;
        private readonly EcsPool<OwnerComponent> _ownerPool;

        public ItemService(EcsWorld world)
        {
            _world = world;

            _itemPool = _world.GetPool<ItemComponent>();
            _ownerPool = _world.GetPool<OwnerComponent>();
        }

        public int CreateItem(ItemConfig config)
        {
            var entity = _world.NewEntity();

            _itemPool.Add(entity) = new ItemComponent
            {
                Config = config
            };

            return entity;
        }

        public void TryEquip(int itemEntity, EcsPackedEntityWithWorld ownerEntity)
        {
            var item = _itemPool.Get(itemEntity);
            var equippedItems = GetAllEquippedItems();

            if (!equippedItems.ContainsKey(item.Config.slot))
                Equip(itemEntity, ownerEntity);
            else
                Swap();
        }

        private void Equip(int itemEntity, EcsPackedEntityWithWorld ownerEntity)
        {
            _ownerPool.Add(itemEntity) = new OwnerComponent
            {
                Entity = ownerEntity
            };
        }

        private void Swap()
        {
        }

        public void ActivateItem(int entity)
        {
            throw new System.NotImplementedException();
        }

        public void UseItem(int entity)
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<EItemSlot, int> GetAllEquippedItems()
        {
            var filter = _world.Filter<ItemComponent>().Inc<OwnerComponent>().End();
            var result = new Dictionary<EItemSlot, int>();

            foreach (var entity in filter)
            {
                var item = _itemPool.Get(entity);
                result.Add(item.Config.slot, entity);
            }

            return result;
        }
    }
}