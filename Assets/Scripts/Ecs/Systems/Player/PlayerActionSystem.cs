using CyberNinja.Models;
using CyberNinja.Models.Data;
using CyberNinja.Services;
using CyberNinja.Services.Impl;
using CyberNinja.Services.Unit;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CyberNinja.Ecs.Systems.Player
{
    public class PlayerActionSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<GameData> _gameData;
        private readonly EcsCustomInject<UnitService> _unitService;
        private readonly EcsCustomInject<ItemService> _itemService;
        private readonly EcsCustomInject<PlayerService> _playerService;

        public void Run(IEcsSystems systems)
        {
            var input = _gameData.Value.Input;
            input._Player.Use.performed += OnUse;
        }

        private void OnUse(InputAction.CallbackContext obj)
        {
            var playerEntity = _playerService.Value.GetEntity();
            var trigger = _unitService.Value.GetTrigger(playerEntity);
            if (trigger == null)
                return;
            
            _itemService.Value.Pickup(trigger as ItemView);
        }
    }
}