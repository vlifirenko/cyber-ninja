using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Models.Data;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CyberNinja.Ecs.Systems.Lobby.Upgrade
{
    public class SelectUnitPartSystem : IEcsRunSystem
    {
        private EcsCustomInject<LobbyData> _lobbyData;
        private EcsCustomInject<LobbySceneView> _sceneView;
        private EcsCustomInject<LobbyConfig> _mineConfig;
        
        public void Run(IEcsSystems systems)
        {
            var ray = _sceneView.Value.UpgradeCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity/*, _mineConfig.Value.unitPartLayer*/))
            {
                Debug.Log(hit.transform.name);
                Debug.Log("hit");
            }
        }
    }
}