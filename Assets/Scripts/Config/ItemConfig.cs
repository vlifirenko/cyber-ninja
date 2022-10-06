using UnityEngine;

namespace CyberNinja.Config
{
    [CreateAssetMenu(menuName = "Config/Item", fileName = "Item")]
    public class ItemConfig : ScriptableObject
    {
        public bool tryEquip;
    }
}