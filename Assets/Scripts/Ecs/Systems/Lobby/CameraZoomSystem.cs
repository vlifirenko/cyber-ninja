using CyberNinja.Ecs.Components.Lobby;
using CyberNinja.Models.Config;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Lobby
{
    public class CameraZoomSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<CameraZoomComponent>> _filter;
        private EcsPoolInject<CameraZoomComponent> _pool;
        private EcsCustomInject<LobbySceneView> _sceneView;
        private EcsCustomInject<LobbyConfig> _mineConfig;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var cameraZoom = ref _pool.Value.Get(entity);
                var offset = Vector3.Lerp(
                    cameraZoom.Origin,
                    cameraZoom.Target,
                    cameraZoom.Time);

                _sceneView.Value.CameraView.CameraIsometric.GetComponent<CinemachineCameraOffset>().m_Offset = offset;

                if (Vector3.Distance(offset, cameraZoom.Target) < 1f)
                    _pool.Value.Del(entity);
                else
                    cameraZoom.Time += Time.deltaTime * _mineConfig.Value.zoomSpeed;
            }
        }
    }
}