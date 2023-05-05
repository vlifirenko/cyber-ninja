using CyberNinja.Models.Enums;
using CyberNinja.Models.Unit;
using CyberNinja.Views.Unit;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Item", fileName = "Item")]
    public class ItemConfig : ScriptableObject
    {
        public string title;
        public EItemType type;
        public EItemSlot slot;
        public ItemPosition prefab;
        public Sprite icon;
        public AbilityConfig abilityConfig;
        public Push ownerPush;
        public Push targetPush;
    }
}