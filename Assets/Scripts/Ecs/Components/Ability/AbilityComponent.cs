using CyberNinja.Config;
using CyberNinja.Models.Ability;
using Leopotam.EcsLite;

namespace CyberNinja.Ecs.Components.Ability
{
    public struct AbilityComponent
    {
        public int SlotIndex;
        public AbilityConfig AbilityConfig;
        public AbilityOverrideItem[] OverrideData;
        public EcsPackedEntity Owner;
    }
}