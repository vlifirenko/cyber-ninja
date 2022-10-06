using CyberNinja.Views;

namespace CyberNinja.Services
{
    public interface IItemService
    {
        public int CreateItem(ItemView view);
        
        public void ActivateItem(int entity);

        public void PickupItem(ItemView view);
    }
}