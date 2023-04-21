using CyberNinja.Models.Enums;
using CyberNinja.Views;
using CyberNinja.Views.Ui;

namespace CyberNinja.Ecs.Components.Unit
{
    public struct EnemyComponent
    {
        public UiHealthSlider HealthSlider;
        public RoomView Room;
        public EEnemyType Type;
    }
}