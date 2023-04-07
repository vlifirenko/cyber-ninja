using CyberNinja.Models.Config;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CyberNinja.Ecs.Systems.Mine
{
    public class MineCellMouseHoverSystem : IEcsRunSystem
    {
        private EcsCustomInject<MineConfig> _mineConfig;

        private MineCell _prevMineCell;

        public void Run(IEcsSystems systems)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _mineConfig.Value.mineCellLayer))
            {
                var mineCell = hit.transform.parent.GetComponent<MineCell>();
                if (!mineCell.IsHovered)
                    mineCell.IsHovered = true;

                if (_prevMineCell == null)
                    _prevMineCell = mineCell;
                else if (_prevMineCell != null && _prevMineCell != mineCell)
                {
                    _prevMineCell.IsHovered = false;
                    _prevMineCell = mineCell;
                }
            }
            else if (_prevMineCell != null)
            {
                _prevMineCell.IsHovered = false;
                _prevMineCell = null;
            }
        }
    }
}