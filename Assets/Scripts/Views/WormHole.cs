using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class WormHole : AView
    {
        [SerializeField] private Camera wormHoleCamera;
        [SerializeField] private MeshRenderer meshRenderer;

        public Camera Camera => wormHoleCamera;

        public MeshRenderer Renderer => meshRenderer;
    }
}