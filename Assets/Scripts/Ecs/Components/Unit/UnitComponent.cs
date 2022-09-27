using CyberNinja.Models.Enums;
using CyberNinja.Views.Unit;

namespace CyberNinja.Ecs.Components.Unit
{
    public struct UnitComponent
    {
        public UnitView View;
        public EControlType ControlType;
    }
}