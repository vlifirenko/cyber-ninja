using UnityEngine;

namespace CyberNinja.Ecs.Components.Unit
{
    public struct DashComponent
    {
        public float TimeLeft;
        public float Time;
        public float Distance;
        public Vector3 Vector;
        public bool UseLook;
        public bool VectorActivated;
    }
}