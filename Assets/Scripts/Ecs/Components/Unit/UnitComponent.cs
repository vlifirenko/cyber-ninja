using CyberNinja.Models.Config;
using CyberNinja.Views.Ui;
using CyberNinja.Views.Unit;

namespace CyberNinja.Ecs.Components.Unit
{
    public struct UnitComponent
    {
        public UnitView View;
        public UnitConfig Config;
    }
}