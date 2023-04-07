using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Mine", fileName = "Mine")]
    public class MineConfig : ScriptableObject
    {
        public bool isOuterCircleUnlocked;
    }
}