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
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using Observable = UniRx.Observable;

namespace CyberNinja.Ecs.Systems.Mine
{
    public class MineCellMouseSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsCustomInject<LobbyConfig> _mineConfig;
        private EcsCustomInject<GameData> _gameData;
        private EcsCustomInject<SaveService> _saveService;

        private MineCell _hoveredMineCell;
        private MineCell _selectedMineCell;

        [EcsUguiNamed(UiConst.MinePopup)] private MinePopup _minePopup;
        [EcsUguiNamed(UiConst.MessageText)] private TMP_Text _messageText;
        [EcsUguiNamed(UiConst.Canvas)] private Canvas _canvas;

        public void Init(IEcsSystems systems)
        {
            var controls = new Controls();
            _gameData.Value.Input = controls;

            controls.Mine.Enable();
            controls.Mine.Select.performed += OnMouseClick;

            _minePopup.UpgradeButton.onClick.AddListener(OnMineUpgradeButton);
        }

        public void Run(IEcsSystems systems)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _mineConfig.Value.mineCellLayer))
            {
                var mineCell = hit.transform.parent.GetComponent<MineCell>();
                if (mineCell.MineCircle == EMineCircle.Core)
                {
                    if (_hoveredMineCell != null)
                        UnhoverMineCell(null);
                }

                if (!mineCell.IsHovered)
                    mineCell.IsHovered = true;

                if (_hoveredMineCell == null)
                    _hoveredMineCell = mineCell;
                else if (_hoveredMineCell != null && _hoveredMineCell != mineCell)
                    UnhoverMineCell(mineCell);
            }
            else if (_hoveredMineCell != null)
                UnhoverMineCell(null);
        }

        private void UnhoverMineCell(MineCell newCell)
        {
            _minePopup.Inner.gameObject.SetActive(false);
            _hoveredMineCell.IsHovered = false;
            _hoveredMineCell = newCell;
        }

        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (_hoveredMineCell == null)
                return;

            _selectedMineCell = _hoveredMineCell;

            var position = _canvas.WorldToCanvasPosition(_selectedMineCell.Transform.position);
            position += new Vector3(_minePopup.Offset.x, _minePopup.Offset.y);
            _minePopup.Window.anchoredPosition = position;
            Observable.Timer(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ => _minePopup.Inner.gameObject.SetActive(true));
        }

        private void OnMineUpgradeButton()
        {
            if (_selectedMineCell == null)
                return;
            if (_selectedMineCell.MineCellState == EMineCellState.Level3)
                return;

            var resourcesLeft = _gameData.Value.playerResources.Get(EResourceType.Cobalt);
            if (_selectedMineCell.MineCellState == EMineCellState.Level1)
            {
                if (resourcesLeft < _mineConfig.Value.mineUpgrade2Cost)
                {
                    _messageText.text = $"Not enough {EResourceType.Cobalt}";
                    Observable.Timer(TimeSpan.FromSeconds(2))
                        .Subscribe(_ => _messageText.text = "");
                    return;
                }

                UpgradeMineRoom(EResourceType.Cobalt, _mineConfig.Value.mineUpgrade2Cost,
                    _selectedMineCell, EMineCellState.Level2);
            }
            else if (_selectedMineCell.MineCellState == EMineCellState.Level2)
            {
                if (resourcesLeft < _mineConfig.Value.mineUpgrade3Cost)
                {
                    _messageText.text = $"Not enough {EResourceType.Cobalt}";
                    Observable.Timer(TimeSpan.FromSeconds(2))
                        .Subscribe(_ => _messageText.text = "");
                    return;
                }

                UpgradeMineRoom(EResourceType.Cobalt, _mineConfig.Value.mineUpgrade3Cost,
                    _selectedMineCell, EMineCellState.Level3);
            }
        }

        private void UpgradeMineRoom(EResourceType resourceType, float cost, MineCell cell, EMineCellState state)
        {
            cell.MineCellState = state;
            _gameData.Value.playerResources.Update(resourceType,-cost);
            if (cell.MineCircle == EMineCircle.Inner)
                _gameData.Value.mine.innerCircle.Update(cell.Index, state);
            if (cell.MineCircle == EMineCircle.Outer)
                _gameData.Value.mine.outerCircle.Update(cell.Index, state);
            
            SaveService.Save(_gameData.Value);
        }
    }
}