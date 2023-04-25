using System;
using CyberNinja.Models.Config;
using UnityEngine.Serialization;

namespace CyberNinja.Models
{
    [Serializable]
    public class ArmyUnit
    {
        public UnitConfig config;
        public float currentHealth;
        public int exp;
        public int expMax;
        public int level;
        public SkillState attackSkill = new SkillState();
        public SkillState blockSkill = new SkillState();
    }

    [Serializable]
    public class SkillState
    {
        public float value = 10f;
        public int level = 1;
        public float exp;
        public float expMax = 100f;
    }
}