using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Models.Data;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class CollectLootSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<PlayerComponent>, Exc<DeadComponent>> _filter;
        private EcsWorldInject _world;
        private EcsCustomInject<GlobalUnitConfig> _globalUnitConfig;
        private EcsCustomInject<LayersConfig> _layersConfig;
        private EcsCustomInject<GameData> _gameData;

        private int _colliders;

        public void Init(IEcsSystems systems)
        {
            var input = _gameData.Value.Input;

            input._Player.Use.performed += OnUse;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var unit = _world.Value.GetPool<UnitComponent>().Get(entity);
                var position = unit.View.Transform.position;
                
                var colliders = Physics.SphereCastAll(position, _globalUnitConfig.Value.lootRange,
                    unit.View.Transform.forward, 0f, _layersConfig.Value.loot);

                _colliders = colliders.Length;
            }
        }

        private void OnUse(InputAction.CallbackContext obj) => Debug.Log(_colliders);
    }
}