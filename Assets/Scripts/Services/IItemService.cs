using CyberNinja.Models.Config;

namespace CyberNinja.Services
{
    public interface IItemService
    {
        public void ActivateItem(int entity);

        public void TryEquip(ItemConfig config);

        public void UseItem(int entity);
    }
}