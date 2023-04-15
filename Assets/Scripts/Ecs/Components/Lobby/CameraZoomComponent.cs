using UnityEngine;

namespace CyberNinja.Ecs.Components.Lobby
{
    public struct CameraZoomComponent
    {
        public Vector3 Origin;
        public Vector3 Target;
        public Vector3 Current;
        public float Time;
    }
}