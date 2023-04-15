using System.Linq;
using CyberNinja.Ecs.Components.Room;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
using CyberNinja.Models.Enums;
using CyberNinja.Services;
using CyberNinja.Services.Unit;
using CyberNinja.Utils;
using CyberNinja.Views;
using CyberNinja.Views.Ui;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CyberNinja.Ecs.Systems.Room
{
    public class UpdateRoomSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<UpdateRoomComponent>> _filter;
        private EcsPoolInject<UpdateRoomComponent> _updateRoomPool;
        private readonly EcsWorldInject _world;
        private readonly EcsCustomInject<SceneView> _sceneView;
        private readonly EcsCustomInject<IAiService> _aiService;
        private readonly EcsCustomInject<UnitService> _unitService;
        private EcsCustomInject<GlobalUnitConfig> _globalUnitConfig;

        [EcsUguiNamed(UiConst.HealthSliderContainer)]
        private UiHealthSliderContainer _healthSliderContainer;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var updateRoom = _updateRoomPool.Value.Get(entity);
                var room = updateRoom.Room;

                if (updateRoom.IsSpawnEnemy)
                    SpawnEnemies(room);
                else if (updateRoom.IsRoomClear)
                    RoomClear(room);
            }
        }

        private void SpawnEnemies(RoomView room)
        {
            foreach (var enemyItem in room.RoomConfig.enemies)
            {
                for (var i = 0; i < enemyItem.amount; i++)
                    SpawnEnemy(room, enemyItem.type);
            }
                
        }

        private void SpawnEnemy(RoomView room, EEnemyType type)
        {
            var spawnPoints = room.EnemySpawnPoints.ToList();
            if (spawnPoints.Count == 0)
            {
                Debug.LogError("spawnPoints.Count == 0");
                return;
            }

            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            spawnPoints.Remove(spawnPoint);

            var enemies = _sceneView.Value.Enemies.Where(item => item.type == type);
            foreach (var item in enemies)
            {
                var instance = Object.Instantiate(item.view, spawnPoint.position, spawnPoint.rotation,
                    _sceneView.Value.UnitContainer.Transform);
                var entity = _unitService.Value.CreateUnit(instance);
                var enemyPool = _world.Value.GetPool<EnemyComponent>();
                var enemy = new EnemyComponent();
                var uiSlider = Object.Instantiate(_healthSliderContainer.Prefab, _healthSliderContainer.Container);

                enemy.Type = item.type;
                enemy.Room = room;
                enemy.HealthSlider = uiSlider;
                uiSlider.gameObject.SetActive(true);

                enemyPool.Add(entity) = enemy;
                _aiService.Value.InitUnit(entity);

                instance.Show();
            }
        }

        private void RoomClear(RoomView room)
        {
            Debug.Log("RoomClear");
        }
    }
}