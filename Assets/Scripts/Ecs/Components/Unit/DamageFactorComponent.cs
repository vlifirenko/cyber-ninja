using System.Collections.Generic;

namespace CyberNinja.Ecs.Components.Unit
{
    public struct DamageFactorComponent
    {
        public float PhysicalFactor;
        public List<float> ImpactList;
    }
}