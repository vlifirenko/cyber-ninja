using CyberNinja.Views;

namespace CyberNinja.Ecs.Components.Room
{
    public struct UpdateRoomComponent
    {
        public RoomView Room;
        public bool IsSpawnEnemy;
        public bool IsRoomClear;
    }
}