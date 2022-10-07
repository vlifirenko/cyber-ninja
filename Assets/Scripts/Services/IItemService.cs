using CyberNinja.Config;
using CyberNinja.Views;

namespace CyberNinja.Services
{
    public interface IItemService
    {
        public int CreateItem(ItemView view);
        
        public void ActivateItem(int entity);

        public void PickupItem(ItemView view);

        public void TryEquip(ItemConfig config);

        public void UseItem(int entity, ItemConfig config);
    }
}