using System;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Mine", fileName = "Mine")]
    public class MineConfig : ScriptableObject
    {
        public bool isOuterCircleUnlocked;
        public StartResource[] startResources;
        public float outerCircleUnlockCost = 100;
        public int startColonyLevel = 1;
        public LayerMask mineCellLayer;
    }

    [Serializable]
    public class StartResource
    {
        public EResourceType type;
        public float value;
    }
}