using System;
using CyberNinja.Models.Config;

namespace CyberNinja.Models
{
    [Serializable]
    public class ArmyUnit
    {
        public UnitConfig config;
        public float currentHealth;
    }
}