using CyberNinja.Views.Containers;
using CyberNinja.Views.Core;
using CyberNinja.Views.Unit;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering;

namespace CyberNinja.Views
{
    public class SceneView : AView
    {
        [SerializeField] private CameraView cameraView;
        [SerializeField] private UnitView playerView;
        [SerializeField] private StudioEventEmitter fmodEventEmitter;
        [SerializeField] private Volume volume;
        [SerializeField] private DoorContainerView doorContainerView;
        [SerializeField] private UnitContainerView unitContainerView;

        public CameraView CameraView => cameraView;
        public UnitView PlayerView => playerView;
        public StudioEventEmitter FmodEventEmitter => fmodEventEmitter;
        public Volume Volume => volume;
        public DoorContainerView DoorContainerView => doorContainerView;
        public UnitContainerView UnitContainerView => unitContainerView;
    }
}