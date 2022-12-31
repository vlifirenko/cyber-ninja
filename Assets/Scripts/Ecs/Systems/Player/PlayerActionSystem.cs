using CyberNinja.Services;
using CyberNinja.Services.Unit;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.InputSystem;

namespace CyberNinja.Ecs.Systems.Player
{
    public class PlayerActionSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<IPlayerService> _playerService;
        private readonly EcsCustomInject<IUnitService> _unitService;
        private readonly EcsCustomInject<IItemService> _itemService;

        public void Run(IEcsSystems systems)
        {
            var input = _playerService.Value.GetInput();
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