using System.Collections.Generic;
using CyberNinja.Ecs.Components.Item;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
using CyberNinja.Models.Enums;
using CyberNinja.Views.Unit;
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

            if (ownerEntity.Unpack(out var world, out var entity))
            {
                var unit = world.GetPool<UnitComponent>().Get(entity);
                var item = _itemPool.Get(itemEntity);
                var weaponView = unit.View.WeaponSlotView;

                if (!weaponView.IsWeaponEnabled)
                    return;

                if (weaponView.ItemInstance != null)
                    Object.Destroy(weaponView.ItemInstance.gameObject);

                var hand = weaponView.UseRightHand ? weaponView.HandRight : weaponView.HandLeft;
                var weaponInstance = Object.Instantiate(item.Config.prefab, hand, false);

                weaponInstance.Transform.localPosition = weaponInstance.Position;
                weaponInstance.Transform.localRotation = Quaternion.Euler(weaponInstance.Rotation);
                weaponInstance.Transform.localScale = weaponInstance.Scale;

                weaponView.ItemInstance = weaponInstance;
            }
        }

        private void Swap()
        {
            Debug.Log("Swap");
            
            // remove
            // todo
            /*var unit = _unitPool.Value.Get(entity);
            var weaponView = unit.View.WeaponSlotView;

            for (var i = 0; i < weaponView.WeaponsContainer.childCount; i++)
            {
                Object.DestroyImmediate(weaponView.WeaponsContainer
                    .GetChild(weaponView.WeaponsContainer.childCount - i - 1).gameObject);
            }*/
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

        public void Pickup(ItemView view)
        {
            var config = view.Config;
            Debug.Log($"pickup {config.title}");
        }
    }
}