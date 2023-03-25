using CyberNinja.Models.Config;
using CyberNinja.Views.Unit;

namespace CyberNinja.Ecs.Components.Unit
{
    public struct WeaponComponent
    {
        public ItemConfig Config;
        public ItemView SceneView;
    }
}