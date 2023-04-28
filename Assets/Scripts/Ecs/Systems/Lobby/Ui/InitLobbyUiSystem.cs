using CyberNinja.Ecs.Components.Lobby;
using CyberNinja.Ecs.Systems.Ui;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Utils;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Ecs.Systems.Lobby.Ui
{
    public class InitLobbyUiSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsCustomInject<LobbyData> _lobbyData;
        private EcsCustomInject<LobbySceneView> _sceneView;
        private EcsCustomInject<LobbyConfig> _lobbyConfig;
        private EcsFilterInject<Inc<CameraZoomComponent>> _filter;
        private EcsWorldInject _world;

        [EcsUguiNamed(UiConst.HangarWindow)] private UiHangarWindow _hangarWindow;
        [EcsUguiNamed(UiConst.ZoomOutButton)] private Button _zoomOutButton;
        [EcsUguiNamed(UiConst.ZoomInButton)] private Button _zoomInButton;

        private int _selectedArmyUnit;
        private bool _isCameraMoving;
        private Vector3 _cameraMovingOrigin;
        private Vector3 _cameraMovingTarget;
        private float _cameraMovingTime;

        public void Init(IEcsSystems systems)
        {
            _zoomInButton.interactable = false;
            
            _hangarWindow.OpenFullButton.onClick.AddListener(OnHangarOpenFull);
            _hangarWindow.CloseButton.onClick.AddListener(OnHangarClose);

            for (var i = 0; i < _hangarWindow.ArmyButtons.Length; i++)
            {
                var button = _hangarWindow.ArmyButtons[i];
                var position = i;
                button.onClick.AddListener(() => OnArmyUnit(position));
            }

            UpdateUnitInfo(_lobbyData.Value.selectedArmyUnit);
            UpdateUnitSkills(_lobbyData.Value.selectedArmyUnit);
        }

        public void Run(IEcsSystems systems)
        {
            if (_isCameraMoving)
            {
                _cameraMovingTime += Time.deltaTime;
                var position = Vector3.Lerp(
                    _cameraMovingOrigin,
                    _cameraMovingTarget,
                    _cameraMovingTime * _lobbyConfig.Value.hangarCameraSpeed
                );

                _sceneView.Value.Hangar.HangarCamera.transform.position = position;

                if (Vector3.Distance(_cameraMovingTarget, _sceneView.Value.Hangar.HangarCamera.transform.position) < .1f)
                {
                    _sceneView.Value.Hangar.HangarCamera.transform.position = _cameraMovingTarget;
                    _isCameraMoving = false;

                    _lobbyData.Value.selectedArmyUnit = _lobbyData.Value.Army[_selectedArmyUnit];
                    UpdateUnitInfo(_lobbyData.Value.selectedArmyUnit);
                    UpdateUnitSkills(_lobbyData.Value.selectedArmyUnit);
                }
            }
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

        private void OnArmyUnit(int position)
        {
            if (position == _selectedArmyUnit)
                return;

            var hangar = _sceneView.Value.Hangar;

            _isCameraMoving = true;
            _cameraMovingOrigin = hangar.HangarCamera.transform.position;
            _cameraMovingTarget = hangar.CameraPositions[position].transform.position;
            _cameraMovingTime = 0f;
            
            _selectedArmyUnit = position;
        }

        private void UpdateUnitInfo(ArmyUnit unit)
        {
            _hangarWindow.UnitNameText.text = unit.unitConfig.name;
            _hangarWindow.UnitNameText.color = unit.armyConfig.color;
            _hangarWindow.UnitLevelText.text = $"LEVEL: {unit.level}";
            _hangarWindow.UnitExpSlider.value = (float)unit.exp / unit.expMax;
            _hangarWindow.UnitExpText.text = $"{unit.exp}/{unit.expMax}";
        }
        
        private void UpdateUnitSkills(ArmyUnit unit)
        {
            _hangarWindow.AttackSkill.NameText.text = $"ATTACK: {unit.attackSkill.value}";
            _hangarWindow.AttackSkill.LevelText.text = $"Level: {unit.attackSkill.level}";
            _hangarWindow.AttackSkill.Slider.value = unit.attackSkill.exp / unit.attackSkill.expMax;
            _hangarWindow.AttackSkill.SliderText.text = $"{Mathf.CeilToInt(unit.attackSkill.exp / unit.attackSkill.expMax)}%";
            
            _hangarWindow.BlockSkill.NameText.text = $"BLOCK: {unit.blockSkill.value}";
            _hangarWindow.BlockSkill.LevelText.text = $"Level: {unit.blockSkill.level}";
            _hangarWindow.BlockSkill.Slider.value = unit.blockSkill.exp / unit.blockSkill.expMax;
            _hangarWindow.BlockSkill.SliderText.text = $"{Mathf.CeilToInt(unit.blockSkill.exp / unit.blockSkill.expMax)}%";
            
            _hangarWindow.UnitLevelText.text = $"LEVEL: {unit.level}";
            _hangarWindow.UnitExpSlider.value = (float)unit.exp / unit.expMax;
            _hangarWindow.UnitExpText.text = $"{unit.exp}/{unit.expMax}";
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