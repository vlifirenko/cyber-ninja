using System;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Mine", fileName = "Mine")]
    public class MineConfig : ScriptableObject
    {
        public bool isOuterCircleUnlocked;
        public StartResource[] startResources;
    }

    [Serializable]
    public class StartResource
    {
        public EResourceType type;
        public float value;
    }
}