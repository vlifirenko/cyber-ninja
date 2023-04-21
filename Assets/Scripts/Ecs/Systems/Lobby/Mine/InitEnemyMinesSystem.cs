using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Lobby.Mine
{
    public class InitEnemyMinesSystem : IEcsInitSystem
    {
        private EcsCustomInject<LobbySceneView> _sceneView;
        private EcsCustomInject<LobbyConfig> _lobbyConfig;
        private EcsCustomInject<LobbyData> _lobbyData;

        public void Init(IEcsSystems systems)
        {
            var enemyCount = Random.Range(_lobbyConfig.Value.startEnemyCount.x, _lobbyConfig.Value.startEnemyCount.y);
            for (var i = 0; i < enemyCount; i++)
            {
                var enemy = new LobbyEnemy
                {
                    view = CreateEnemyView(),
                    username = GenerateUsername()
                };

                _lobbyData.Value.lobbyEnemies.Add(enemy);
            }
        }

        private LobbyMine CreateEnemyView()
        {
            var prefab = _sceneView.Value.MinesView.EnemyPrefab;
            var container = _sceneView.Value.MinesView.EnemyContainer;
            var position = new Vector3(
                Random.Range(-_lobbyConfig.Value.mineOffset.x, _lobbyConfig.Value.mineOffset.x),
                0.75f,
                Random.Range(-_lobbyConfig.Value.mineOffset.y, _lobbyConfig.Value.mineOffset.y)
            );
            var rotation = Quaternion.Euler(
                0f,
                Random.Range(0f, 360f),
                0f);
            var instance = Object.Instantiate(prefab, position, rotation, container);
            
            return instance;
        }

        private string GenerateUsername()
        {
            return "name";
        }
    }
}