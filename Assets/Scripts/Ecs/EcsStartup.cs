using CyberNinja.Config;
using CyberNinja.Ecs.Systems;
using CyberNinja.Ecs.Systems.Ability;
using CyberNinja.Ecs.Systems.Ai;
using CyberNinja.Ecs.Systems.Door;
using CyberNinja.Ecs.Systems.Game;
using CyberNinja.Ecs.Systems.Item;
using CyberNinja.Ecs.Systems.Unit;
using CyberNinja.Models;
using CyberNinja.Services;
using CyberNinja.Services.Impl;
using CyberNinja.Utils;
using CyberNinja.Views;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;

namespace CyberNinja.Ecs
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private SceneView sceneView;
        [SerializeField] private CanvasView canvasView;
        [SerializeField] private EcsUguiEmitter uguiEmitter;
        [SerializeField] private LayersConfig layersConfig;
        [SerializeField] private UnitConfig unitConfig;
        [SerializeField] private AudioConfig audioConfig;
        [SerializeField] private InputConfig inputConfig;

        private EcsSystems _systems;

        private IAiService _aiService;
        private IUnitService _unitService;
        private IAbilityService _abilityService;
        private IDoorService _doorService;
        private IVfxService _vfxService;
        private IItemService _itemService;
        private IGameService _gameService;
        private ITimeService _timeService;

        private GameData _gameData;

        private void Start()
        {
            var world = new EcsWorld();
            var eventWorld = new EcsWorld();
            var itemsWorld = new EcsWorld();

            _gameData = new GameData();
            
            EcsPhysicsEvents.ecsWorld = world;

            _vfxService = new VfxService(world);
            _itemService = new ItemService(itemsWorld);
            _unitService = new UnitService(world, unitConfig, canvasView, _vfxService);
            _doorService = new DoorService(world, _unitService);
            _abilityService = new AbilityService(world, unitConfig, layersConfig, _unitService, _doorService, _vfxService,
                _itemService);
            _aiService = new AiService(world, _abilityService);
            _gameService = new GameService(world, sceneView, canvasView, _gameData);
            _timeService = new TimeService();

            _systems = new EcsSystems(world);
            _systems

                // init
                .Add(new InitUnitsSystem())
                .Add(new InitWeaponSystem())
                .Add(new InitInputSystem())
                .Add(new InitUiSystem())

                // game
                .Add(new GameSystem())
                .Add(new CameraMovementSystem())
                .Add(new AudioSystem())
                .Add(new TimeSystem())
                
                // trigger
                .Add(new UnitTriggerEnterSystem())

                // movement
                .Add(new StunSystem())
                .Add(new MovementSystem())
                .Add(new VectorLookSystem())
                .Add(new StationarySystem())
                .Add(new DashSystem())

                // ai
                .Add(new AiUpdateStateSystem())
                .Add(new AiIdleSystem())
                .Add(new AiChaseSystem())
                .Add(new AiAttackSystem())

                // animation
                .Add(new AnimationSystem())
                .Add(new AnimationLayersSystem())

                // other
                .Add(new AbilityInputBlockSystem())
                .Add(new HealthRegenerationSystem())
                .Add(new EnergyRegenerationSystem())
                .Add(new DamageFactorSystem())
                .Add(new ReviveSystem())
                .Add(new UiHealthSystem())

                // ability
                .Add(new InitAbilitiesSystem())
                .Add(new AbilityCooldownSystem())

                // doors
                .Add(new InitDoorsSystem())
                
                // items
                .Add(new InitItemsSystem())
                
                .AddWorld(eventWorld, World.Events)
                .AddWorld(itemsWorld, World.Items)
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(World.Events))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(World.Items))
#endif
                .Inject(_gameData, sceneView, canvasView)
                .Inject(layersConfig, unitConfig, audioConfig, inputConfig)
                .Inject(_unitService, _aiService, _abilityService, _doorService, _vfxService, _itemService,
                    _gameService, _timeService)
                .Inject()
                .InjectUgui(uguiEmitter, World.Events)
                .DelHerePhysics()
                .Init();
        }

        private void Update() => _systems?.Run();

        private void OnDestroy()
        {
            EcsPhysicsEvents.ecsWorld = null;
            _systems?.Destroy();
            _systems?.GetWorld(World.Events)?.Destroy();
            _systems?.GetWorld(World.Items)?.Destroy();
            _systems?.GetWorld()?.Destroy();
            _systems = null;

            _aiService.OnDestroy();
            _vfxService.OnDestroy();
        }
    }
}