using System;
using CyberNinja.Models.Config;

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