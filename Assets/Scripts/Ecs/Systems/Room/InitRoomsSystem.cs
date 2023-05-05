using CyberNinja.Ecs.Components.Room;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Services.Unit;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Room
{
    public class InitRoomsSystem : IEcsInitSystem
    {
        private EcsCustomInject<SceneView> _sceneView;
        private EcsCustomInject<UnitService> _unitService;
        private EcsCustomInject<GlobalUnitConfig> _globalUnitConfig;
        private EcsCustomInject<GameData> _gameData;
        private EcsWorldInject _world;

        private RoomView _startRoom;

        public void Init(IEcsSystems systems)
        {
            SpawnRooms();
            SpawnPlayer();
            Debug_SpawnEnemies();
            InitStartRoom();
        }

        private void SpawnRooms()
        {
            var prefab = _sceneView.Value.RoomPrefab;
            var container = _sceneView.Value.RoomContainer;

            var instance = Object.Instantiate(prefab, container);
            var position = Vector3.zero;

            instance.Transform.position = position;
            instance.Circle = EMineCircle.Core;

            for (var i = 0; i < 8; i++)
            {
                float xOffset = 0, zOffset = 0;

                instance = Object.Instantiate(prefab, container);
                position = Vector3.zero;

                switch (i)
                {
                    case 0:
                        xOffset = -40f;
                        zOffset = 40f;
                        break;
                    case 1:
                        xOffset = 0f;
                        zOffset = 40f;
                        break;
                    case 2:
                        xOffset = 40f;
                        zOffset = 40f;
                        break;
                    case 3:
                        xOffset = -40f;
                        zOffset = 0f;
                        break;
                    case 4:
                        xOffset = 40f;
                        zOffset = 0f;
                        break;
                    case 5:
                        xOffset = -40f;
                        zOffset = -40f;
                        break;
                    case 6:
                        xOffset = 0f;
                        zOffset = -40f;
                        break;
                    case 7:
                        xOffset = 40f;
                        zOffset = -40f;
                        break;
                }

                position.x = xOffset;
                position.z = zOffset;
                instance.Transform.position = position;
                instance.Circle = EMineCircle.Inner;
                instance.Index = i;

                if (i == 0)
                    _startRoom = instance;
                
                _gameData.Value.rooms.Add(instance);
            }
        }

        private void SpawnPlayer()
        {
            var playerSpawn = _startRoom.PlayerSpawn;
            
            var instance = Object.Instantiate(_sceneView.Value.PlayerView,  playerSpawn.position, playerSpawn.rotation);
            instance.Transform.parent = _sceneView.Value.UnitContainer.Transform;

            var entity = _unitService.Value.CreateUnit(instance);
            var playerPool = _world.Value.GetPool<PlayerComponent>();
            playerPool.Add(entity);
            _unitService.Value.Player = _world.Value.PackEntity(entity);

            _sceneView.Value.CameraView.Target = instance.Transform;

            if (instance.Config.isHasDroid)
                CreateDroid(entity);
        }

        private void Debug_SpawnEnemies()
        {
            foreach (var unitView in _sceneView.Value.UnitContainer.Items)
                _unitService.Value.CreateUnit(unitView);
        }

        private void CreateDroid(int entity)
        {
            var unit = _world.Value.GetPool<UnitComponent>().Get(entity);
            var position = new Vector3(
                Random.Range(-1f, 1f),
                _globalUnitConfig.Value.droidYSpawnPosition,
                Random.Range(-1f, 1f));

            unit.View.DroidView.Transform.localPosition = position;
            unit.View.DroidView.Transform.parent = null;
            unit.View.DroidView.Show();


            _world.Value.GetPool<DroidComponent>().Add(entity).DroidView = unit.View.DroidView;
        }

        private void InitStartRoom()
        {
            var entity = _world.Value.NewEntity();
            _world.Value.GetPool<UpdateRoomComponent>().Add(entity) = new UpdateRoomComponent
            {
                Room = _startRoom,
                IsSpawnEnemy = true
            };
        }
    }
}