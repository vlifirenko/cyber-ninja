using CyberNinja.Ecs.Systems.Mine;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Services;
using CyberNinja.Services.Impl;
using CyberNinja.Views;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;

namespace CyberNinja.Ecs
{
    public class MineEcsStartup : MonoBehaviour
    {
        [SerializeField] private MineSceneView sceneView;
        [SerializeField] private MineConfig mineConfig;
        [SerializeField] private EcsUguiEmitter uguiEmitter;
        [SerializeField] private LayersConfig layersConfig;
        [SerializeField] private GlobalUnitConfig globalUnitConfig;
        [SerializeField] private AudioConfig audioConfig;
        [SerializeField] private InputConfig inputConfig;

        private EcsSystems _systems;

        private VfxService _vfxService;
        private ITimeService _timeService;
        private PlayerService _playerService;

        private GameData _gameData;

        private void Start()
        {
            var world = new EcsWorld();

            _gameData = new GameData();

            EcsPhysicsEvents.ecsWorld = world;

            _vfxService = new VfxService(world);
            _timeService = new TimeService();
            _playerService = new PlayerService(world);

            _systems = new EcsSystems(world);
            _systems

                // init
                .Add(new InitMineSystem())

                // game
                
                // ui
                .Add(new UiUpdateResources())
                
                //
                //.DelHere<PickupComponent>()
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(_gameData, sceneView)
                .Inject(layersConfig, globalUnitConfig, audioConfig, inputConfig, mineConfig)
                .Inject(_timeService, _playerService)
                .Inject()
                .InjectUgui(uguiEmitter)
                .DelHerePhysics()
                .Init();
        }

        private void Update() => _systems?.Run();

        private void OnDestroy()
        {
            EcsPhysicsEvents.ecsWorld = null;
            _systems?.GetWorld()?.Destroy();
            _systems?.Destroy();
            _systems = null;

            _vfxService.OnDestroy();
        }
    }
}