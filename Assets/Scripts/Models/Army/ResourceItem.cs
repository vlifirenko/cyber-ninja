using System;
using CyberNinja.Models.Enums;

namespace CyberNinja.Models.Army
{
    [Serializable]
    public class ResourceItem
    {
        public EResourceType type;
        public float value;
    }
}