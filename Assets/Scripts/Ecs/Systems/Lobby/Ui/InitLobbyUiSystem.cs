using CyberNinja.Ecs.Components.Lobby;
using CyberNinja.Ecs.Systems.Ui;
using CyberNinja.Models;
using CyberNinja.Utils;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Ecs.Systems.Lobby.Ui
{
    public class InitLobbyUiSystem : IEcsInitSystem
    {
        private EcsCustomInject<LobbyData> _lobbyData;
        private EcsCustomInject<LobbySceneView> _sceneView;
        private EcsFilterInject<Inc<CameraZoomComponent>> _filter;
        private EcsWorldInject _world;

        [EcsUguiNamed(UiConst.HangarWindow)] private UiHangarWindow _hangarWindow;
        [EcsUguiNamed(UiConst.ZoomOutButton)] private Button _zoomOutButton;
        [EcsUguiNamed(UiConst.ZoomInButton)] private Button _zoomInButton;

        public void Init(IEcsSystems systems)
        {
            var armyUnit = _lobbyData.Value.army[0];

            //_zoomOutButton.onClick.AddListener(ZoomOut);
            //_zoomInButton.onClick.AddListener(ZoomIn);

            _zoomInButton.interactable = false;
            
            _hangarWindow.OpenFullButton.onClick.AddListener(OnHangarOpenFull);
            _hangarWindow.CloseButton.onClick.AddListener(OnHangarClose);
        }

        private void OnHangarOpenFull()
        {
            _hangarWindow.InnerSmall.gameObject.SetActive(false);
            _hangarWindow.InnerFull.gameObject.SetActive(true);
        }

        private void OnHangarClose()
        {
            _hangarWindow.InnerSmall.gameObject.SetActive(true);
            _hangarWindow.InnerFull.gameObject.SetActive(false);
        }

        private void ZoomOut()
        {
            if (_filter.Value.GetEntitiesCount() > 0)
                return;
            
            _zoomInButton.interactable = true;
            _zoomOutButton.interactable = false;

            var entity = _world.Value.NewEntity();
            var cameraOffset = _sceneView.Value.CameraView.CameraIsometric.GetComponent<CinemachineCameraOffset>();
            var currentOffset = cameraOffset.m_Offset;
            _world.Value.GetPool<CameraZoomComponent>().Add(entity) = new CameraZoomComponent
            {
                Current = currentOffset,
                Origin = currentOffset,
                Target = new Vector3(0f, 0f, -300f),
                Time = 0f
            };
        }

        private void ZoomIn()
        {
            if (_filter.Value.GetEntitiesCount() > 0)
                return;
            
            _zoomInButton.interactable = false;
            _zoomOutButton.interactable = true;
            
            var entity = _world.Value.NewEntity();
            var currentOffset = _sceneView.Value.CameraView.CameraIsometric.GetComponent<CinemachineCameraOffset>().m_Offset;
            _world.Value.GetPool<CameraZoomComponent>().Add(entity) = new CameraZoomComponent
            {
                Current = currentOffset,
                Origin = currentOffset,
                Target = new Vector3(0f, 0f, 0f),
                Time = 0f
            };
        }
    }
}