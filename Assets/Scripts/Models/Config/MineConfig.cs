using System;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Mine", fileName = "Mine")]
    public class MineConfig : ScriptableObject
    {
        public StartResource[] startResources;
        public int outerCircleUnlockCost = 100;
        public int startColonyLevel = 1;
        public LayerMask mineCellLayer;
        public float mineUpgrade2Cost = 200f;
        public float mineUpgrade3Cost = 500f;
    }

    [Serializable]
    public class StartResource
    {
        public EResourceType type;
        public float value;
    }
}