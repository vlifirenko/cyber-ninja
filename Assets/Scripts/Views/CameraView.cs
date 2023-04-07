using Cinemachine;
using CyberNinja.Views.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CyberNinja.Views
{
    public class CameraView : AView
    {
        [FoldoutGroup("References"), SerializeField] private Transform cameraBase;
        [FoldoutGroup("References"), SerializeField] private Transform cameraAim;
        [FoldoutGroup("References"), SerializeField] private Transform cameraMain;
        [Space]
        [FoldoutGroup("References"), SerializeField] private CinemachineFreeLook cameraIsometric;

        [BoxGroup("Target"), SerializeField] private Transform target;
        [Space]
        [BoxGroup("Target"), SerializeField, Range(0.001f, 1)] private float defaultSmooth = 0.2f;
        [BoxGroup("Target"), SerializeField, Range(0.001f, 1)] private float blendSmooth = 0.001f;
        [Space]
        [BoxGroup("Target"), SerializeField, Range(0.001f, 1)] private float blendLerp = 0.01f;

        [BoxGroup("Parameters"), Range(.6f, 10), SerializeField] private float zoom = 0.8f;
        
        public Transform CameraMain => cameraMain;
        public Transform CameraBase => cameraBase;
        public Transform CameraAim => cameraAim;
        public CinemachineFreeLook CameraIsometric => cameraIsometric;
        public Transform Target => target;
        public float DefaultSmooth => defaultSmooth;
        public float BlendSmooth => blendSmooth;
        public float BlendLerp => blendLerp;
        public float Zoom => zoom;
    }
}