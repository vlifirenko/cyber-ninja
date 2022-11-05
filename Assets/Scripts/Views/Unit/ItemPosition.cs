using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views.Unit
{
    public class ItemPosition : AView
    {
        [SerializeField] private Transform model;
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 rotation;
        [SerializeField] private Vector3 scale;

        public Vector3 Position => position;
        public Vector3 Rotation => rotation;
        public Vector3 Scale => scale;
    }
}