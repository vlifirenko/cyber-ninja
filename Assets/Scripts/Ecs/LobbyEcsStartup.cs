using CyberNinja.Ecs.Systems.Army;
using CyberNinja.Ecs.Systems.Game;
using CyberNinja.Ecs.Systems.Lobby.Camera;
using CyberNinja.Ecs.Systems.Lobby.Ui;
using CyberNinja.Ecs.Systems.Lobby.Upgrade;
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
    public class LobbyEcsStartup : MonoBehaviour
    {
        [SerializeField] private LobbySceneView sceneView;
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
        private LobbyData _lobbyData;

        private void Start()
        {
            var world = new EcsWorld();

            var saveService = new SaveService();
            _gameData = SaveService.Load();

            _lobbyData = new LobbyData();

            EcsPhysicsEvents.ecsWorld = world;

            _vfxService = new VfxService(world);
            _timeService = new TimeService();
            _playerService = new PlayerService(world);
            var mineService = new MineService();

            _systems = new EcsSystems(world);
            _systems

                // init
                .Add(new InitInputSystem())
                .Add(new InitMineSystem())

                // game
                .Add(new MineCellMouseSystem())
                
                // army
                .Add(new AddFirstArmyUnitSystem())
                
                // ui
                .Add(new InitLobbyUiSystem())
                .Add(new UiUpdateResources())
                .Add(new CameraZoomSystem())
                //.Add(new SelectUnitPartSystem())
                
                //
                //.DelHere<PickupComponent>()
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(_gameData, _lobbyData, sceneView)
                .Inject(layersConfig, globalUnitConfig, audioConfig, inputConfig, mineConfig)
                .Inject(_timeService, _playerService, saveService, mineService)
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