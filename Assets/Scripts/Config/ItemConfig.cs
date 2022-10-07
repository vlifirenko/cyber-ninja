using System;
using CyberNinja.Models.Enums;
using UnityEngine;

namespace CyberNinja.Config
{
    [CreateAssetMenu(menuName = "Config/Item", fileName = "Item")]
    public class ItemConfig : ScriptableObject
    {
        public bool tryEquip;
        public bool destroyAfterPickup = true;
        public bool useOnPickup;
        public ItemUseEffect useEffect;
        public float reloading;
    }

    [Serializable]
    public struct ItemUseEffect
    {
        public EItemUseEffectType type;
        public float value;
    }
}