using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CyberNinja.Models.Unit
{
    [Serializable]
    public class Damage
    {
        public DamageValue value;
        public Transform damageOrigin;
        public Transform attacker;
    }
    
    [Serializable]
    public struct DamageValue
    {
        public List<DamageValueItem> items;
    }
    
    [Serializable]
    public struct DamageValueItem
    {
        public EDamageType type;
        public float value;
    }

    public enum EDamageType
    {
        None = 0,
        Physical = 10,
        Fire = 20,
        Electrical = 30
    }
}