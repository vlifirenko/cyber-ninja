using CyberNinja.Views;

namespace CyberNinja.Services
{
    public interface IDoorService : IDestroyable
    {
        public void CreateDoor(DoorView view);

        public void TryActivateDoor(DoorView view, int unitEntity);
    }
}