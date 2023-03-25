using CyberNinja.Models.Config;
using CyberNinja.Views.Unit;

namespace CyberNinja.Ecs.Components.Unit
{
    public struct PickupComponent
    {
        public ItemConfig ItemConfig;
        public ItemView ItemSceneView;
    }
}