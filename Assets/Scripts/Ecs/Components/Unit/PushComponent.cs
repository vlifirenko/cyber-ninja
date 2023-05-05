using CyberNinja.Models.Unit;
using UnityEngine;

namespace CyberNinja.Ecs.Components.Unit
{
    public struct PushComponent
    {
        public Push Push;
        public Vector3 Directon;
        public float CurrentTime;
    }
}