using UnityEngine;

namespace CyberNinja.Ecs.Components.Unit
{
    public struct PushComponent
    {
        public Vector3 Directon;
        public float Speed;
        public float TargetTime;
        public float CurrentTime;
    }
}