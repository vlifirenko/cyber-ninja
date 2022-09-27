using Leopotam.EcsLite;
using UnityEngine;

namespace CyberNinja.Ecs.Components.Ai
{
    public struct AiTargetComponent
    {
        public EcsPackedEntity Entity;
        public Transform Transform;
        public float Distance;
    }
}