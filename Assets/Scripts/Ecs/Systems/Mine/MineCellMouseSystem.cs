using System;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Utils;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
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
        private EcsCustomInject<MineConfig> _mineConfig;
        private EcsCustomInject<GameData> _gameData;

        private MineCell _hoveredMineCell;
        private MineCell _selectedMineCell;

        [EcsUguiNamed(UiConst.MinePopup)] private MinePopup _minePopup;

        public void Init(IEcsSystems systems)
        {
            var controls = new Controls();
            _gameData.Value.Controls = controls;

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

            Observable.Timer(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ => _minePopup.Inner.gameObject.SetActive(true));
        }

        private void OnMineUpgradeButton()
        {
            if (_selectedMineCell == null)
                return;
            
            Debug.Log($"upgrade {_selectedMineCell.name}");
        }
    }
}