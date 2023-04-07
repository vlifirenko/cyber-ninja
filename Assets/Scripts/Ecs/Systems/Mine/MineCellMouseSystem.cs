using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CyberNinja.Ecs.Systems.Mine
{
    public class MineCellMouseSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsCustomInject<MineConfig> _mineConfig;
        private EcsCustomInject<GameData> _gameData;

        private MineCell _selectedMineCell;

        public void Init(IEcsSystems systems)
        {
            var controls = new Controls();
            _gameData.Value.Controls = controls;
            
            controls.Mine.Enable();
            controls.Mine.Select.performed += OnMouseClick;
        }

        public void Run(IEcsSystems systems)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _mineConfig.Value.mineCellLayer))
            {
                var mineCell = hit.transform.parent.GetComponent<MineCell>();
                if (mineCell.MineCircle == EMineCircle.Core)
                {
                    if (_selectedMineCell != null)
                    {
                        _selectedMineCell.IsHovered = false;
                        _selectedMineCell = null;
                    }
                }

                if (!mineCell.IsHovered)
                    mineCell.IsHovered = true;

                if (_selectedMineCell == null)
                    _selectedMineCell = mineCell;
                else if (_selectedMineCell != null && _selectedMineCell != mineCell)
                {
                    _selectedMineCell.IsHovered = false;
                    _selectedMineCell = mineCell;
                }
            }
            else if (_selectedMineCell != null)
            {
                _selectedMineCell.IsHovered = false;
                _selectedMineCell = null;
            }
        }

        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            Debug.Log(_selectedMineCell);
        }
    }
}