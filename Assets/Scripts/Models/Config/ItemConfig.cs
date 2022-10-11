using CyberNinja.Models.Enums;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Item", fileName = "Item")]
    public class ItemConfig : ScriptableObject
    {
        public string title;
        public EItemSlot slot;
    }
}