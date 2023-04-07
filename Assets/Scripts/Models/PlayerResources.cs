using System;
using System.Collections.Generic;

namespace CyberNinja.Models
{
    [Serializable]
    public class PlayerResources
    {
        public Dictionary<EResourceType, float> Map = new Dictionary<EResourceType, float>();
    }

    public enum EResourceType
    {
        None = 0,
        Resource1 = 10,
        Resource2 = 20,
        Resource3 = 30
    }
}