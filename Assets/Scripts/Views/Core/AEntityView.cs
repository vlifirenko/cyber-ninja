using Leopotam.EcsLite;

namespace CyberNinja.Views.Core
{
    public abstract class AEntityView : AView
    {
        public EcsPackedEntity Entity { get; set; }
    }
}