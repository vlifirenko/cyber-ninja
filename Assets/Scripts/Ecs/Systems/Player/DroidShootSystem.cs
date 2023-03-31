using CyberNinja.Models;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CyberNinja.Ecs.Systems.Player
{
    public class DroidShootSystem : IEcsInitSystem
    {
        private EcsCustomInject<GameData> _gameData;
        
        public void Init(IEcsSystems systems)
        {
            _gameData.Value.Controls._Player.Droid_Shoot.performed += OnDroidShoot;
        }

        private void OnDroidShoot(InputAction.CallbackContext obj)
        {
            Debug.Log("OnDroidShoot");
        }
    }
}