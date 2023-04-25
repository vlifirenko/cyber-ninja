using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class Hangar : AView
    {
        [SerializeField] private GameObject inner;
        [SerializeField] private Transform[] cameraPositions;
        [SerializeField] private Camera hangarCamera;

        public GameObject Inner => inner;

        public Transform[] CameraPositions => cameraPositions;

        public Camera HangarCamera => hangarCamera;
    }
}