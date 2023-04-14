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
        [SerializeField] private DoorContainer doorContainer;
        [SerializeField] private UnitContainer unitContainer;
        [SerializeField] private SceneObjectContainer sceneObjectContainer;
        [SerializeField] private RoomView roomPrefab;
        [SerializeField] private Transform roomContainer;

        public CameraView CameraView => cameraView;
        public UnitView PlayerView => playerView;
        public StudioEventEmitter FmodEventEmitter => fmodEventEmitter;
        public Volume Volume => volume;
        public DoorContainer DoorContainer => doorContainer;
        public UnitContainer UnitContainer => unitContainer;
        public SceneObjectContainer SceneObjectContainer => sceneObjectContainer;

        public RoomView RoomPrefab => roomPrefab;

        public Transform RoomContainer => roomContainer;
    }
}