using System;
using CyberNinja.Models.Enums;

namespace CyberNinja.Models
{
    [Serializable]
    public struct SceneObjectUseEffect
    {
        public EItemUseEffectType type;
        public float value;
    }
}