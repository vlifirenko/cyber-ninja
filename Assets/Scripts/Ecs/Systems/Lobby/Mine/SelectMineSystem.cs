using System;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Services;
using CyberNinja.Utils;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CyberNinja.Ecs.Systems.Lobby.Mine
{
    public class SelectMineSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsCustomInject<LobbyConfig> _lobbyConfig;
        private EcsCustomInject<LayersConfig> _layersConfig;
        private EcsCustomInject<GameData> _gameData;
        private EcsCustomInject<SaveService> _saveService;

        private LobbyMine _hoveredMine;
        private LobbyMine _selectedMine;

        [EcsUguiNamed(UiConst.MinePopup)] private MinePopup _minePopup;
        [EcsUguiNamed(UiConst.MessageText)] private TMP_Text _messageText;
        [EcsUguiNamed(UiConst.Canvas)] private Canvas _canvas;

        public void Init(IEcsSystems systems)
        {
            var controls = new Controls();
            _gameData.Value.Controls = controls;

            controls.Mine.Enable();
            controls.Mine.Select.performed += OnMouseClick;
        }

        public void Run(IEcsSystems systems)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _layersConfig.Value.lobbyMine))
            {
                var mine = hit.transform.parent.GetComponent<LobbyMine>();

                if (!mine.IsHovered)
                    mine.IsHovered = true;

                if (_hoveredMine == null)
                    _hoveredMine = mine;
                else if (_hoveredMine != null && _hoveredMine != mine)
                    UnhoverMineCell(mine);
            }
            else if (_hoveredMine != null)
                UnhoverMineCell(null);
        }

        private void UnhoverMineCell(LobbyMine newMine)
        {
            _minePopup.Inner.gameObject.SetActive(false);
            _hoveredMine.IsHovered = false;
            _hoveredMine = newMine;
        }

        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (_hoveredMine == null)
                return;

            _selectedMine = _hoveredMine;

            /*var position = _canvas.WorldToCanvasPosition(_selectedMine.Transform.position);
            position += new Vector3(_minePopup.Offset.x, _minePopup.Offset.y);
            _minePopup.Window.anchoredPosition = position;
            Observable.Timer(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ => _minePopup.Inner.gameObject.SetActive(true));*/
        }
    }
}