using System;
using System.IO;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
                var view = CreateEnemyView();
                var level = Random.Range(_lobbyConfig.Value.enemyLevelRange.x, _lobbyConfig.Value.enemyLevelRange.y);
                var enemy = new LobbyEnemy
                {
                    view = view,
                    username = GenerateUsername(),
                    level = level
                };

                view.Data = enemy;
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
            const string path = "Assets/Resources/bot_names.txt";
            var reader = new StreamReader(path);
            var text = reader.ReadToEnd();
            var result = text.Split(new string[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            var randomItem = result[Random.Range(0, result.Length)];
            
            reader.Close();

            return randomItem;
        }
    }
}