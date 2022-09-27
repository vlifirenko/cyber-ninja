using UnityEngine;

namespace CyberNinja.Ecs.Components.Unit
{
    public struct VectorsComponent
    {
        public bool IsActiveVectorLook;
        public Vector3 VectorLook;
        public Vector3 VectorDifference;
    }
}