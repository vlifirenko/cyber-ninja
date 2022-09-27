using CyberNinja.Config;
using CyberNinja.Ecs.Systems;
using CyberNinja.Ecs.Systems.Ability;
using CyberNinja.Ecs.Systems.Ai;
using CyberNinja.Ecs.Systems.Door;
using CyberNinja.Ecs.Systems.Game;
using CyberNinja.Ecs.Systems.Unit;
using CyberNinja.Models;
using CyberNinja.Services;
using CyberNinja.Services.Impl;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private SceneView sceneView;
        [SerializeField] private CanvasView canvasView;
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

        private GameData _gameData;

        private void Start()
        {
            var world = new EcsWorld();

            _gameData = new GameData();

            _vfxService = new VfxService(world);
            _itemService = new ItemService();
            _unitService = new UnitService(world, unitConfig, canvasView, _vfxService);
            _doorService = new DoorService(world, _unitService);
            _abilityService = new AbilityService(world, unitConfig, layersConfig, _unitService, _doorService, _vfxService,
                _itemService);
            _aiService = new AiService(world, _abilityService);
            _gameService = new GameService(world, sceneView, canvasView, _gameData);

            _systems = new EcsSystems(world);
            _systems

                // init
                .Add(new InitUnitsSystem())
                .Add(new InitWeaponSystem())
                .Add(new InitInputSystem())

                // game
                .Add(new GameSystem())
                .Add(new CameraMovementSystem())
                .Add(new AudioSystem())

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

#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(_gameData)
                .Inject(sceneView)
                .Inject(canvasView)
                .Inject(layersConfig)
                .Inject(unitConfig)
                .Inject(audioConfig)
                .Inject(inputConfig)
                .Inject(_unitService)
                .Inject(_aiService)
                .Inject(_abilityService)
                .Inject(_doorService)
                .Inject(_vfxService)
                .Inject(_itemService)
                .Inject(_gameService)
                .Inject()
                .Init();
        }

        private void FixedUpdate() => _systems?.Run();

        private void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems.GetWorld().Destroy();
                _systems = null;
            }

            _aiService.OnDestroy();
            _vfxService.OnDestroy();
        }
    }
}