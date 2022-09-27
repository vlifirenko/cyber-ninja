using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Game
{
    public class CameraMovementSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<SceneView> _sceneView;

        private float _finalTargetSmooth;
        private Transform _lastTarget;

        public void Run(IEcsSystems systems)
        {
            var cameraView = _sceneView.Value.CameraView;

            // update
            cameraView.CameraIsometric.m_YAxis.Value = cameraView.Zoom;

            // fixed update
            if (cameraView.Target == null)
                return;
            if (_lastTarget != cameraView.Target)
                _finalTargetSmooth = cameraView.BlendSmooth;

            _finalTargetSmooth = _finalTargetSmooth < cameraView.DefaultSmooth - 0.1f
                ? Mathf.Lerp(_finalTargetSmooth, cameraView.DefaultSmooth, cameraView.BlendLerp)
                : cameraView.DefaultSmooth;

            _lastTarget = cameraView.Target;

            cameraView.CameraBase.position =
                Vector3.Lerp(cameraView.CameraBase.position, cameraView.Target.position, _finalTargetSmooth);
            cameraView.CameraAim.position =
                Vector3.Lerp(cameraView.CameraAim.position, cameraView.Target.position, _finalTargetSmooth);
        }
    }
}