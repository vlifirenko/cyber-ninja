using System.Collections.Generic;
using CyberNinja.Models.Config;
using CyberNinja.Models.Enums;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;

namespace CyberNinja.Services
{
    public interface IItemService
    {
        public int CreateItem(ItemConfig config);
        
        public void ActivateItem(int entity);

        public void TryEquip(int itemEntity, EcsPackedEntityWithWorld ownerEntity);

        public void UseItem(int entity);
        
        public Dictionary<EItemSlot, int> GetAllEquippedItems();

        public void Pickup(ItemView view);
    }
}