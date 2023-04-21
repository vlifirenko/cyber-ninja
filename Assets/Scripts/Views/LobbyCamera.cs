using Cinemachine;
using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class LobbyCamera : AView
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Transform lookPoint;

        public CinemachineVirtualCamera VirtualCamera => virtualCamera;
        public Transform LookPoint => lookPoint;
    }
}