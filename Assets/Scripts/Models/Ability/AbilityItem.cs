using System;
using CyberNinja.Config;

namespace CyberNinja.Models.Ability
{
    [Serializable]
    public class AbilityItem
    {
        public int slotIndex;
        public AbilityConfig abilityConfig;
        public AbilityOverrideItem[] overrideData;
    }
}