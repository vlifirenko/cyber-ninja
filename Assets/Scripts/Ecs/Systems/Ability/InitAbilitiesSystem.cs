using CyberNinja.Ecs.Components;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Data;
using CyberNinja.Services;
using CyberNinja.Services.Impl;
using CyberNinja.Services.Unit;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Ability
{
    public class InitAbilitiesSystem : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<PlayerComponent>> _playerFilter;
        private readonly EcsFilterInject<Inc<EnemyComponent>> _enemyFilter;
        private readonly EcsCustomInject<AbilityService> _abilityService;
        private readonly EcsCustomInject<UnitService> _unitService;
        private readonly EcsCustomInject<CanvasView> _canvasView;
        private readonly EcsPoolInject<PlayerComponent> _playerPool;
        private readonly EcsPoolInject<EnemyComponent> _enemyPool;
        private readonly EcsCustomInject<GameData> _gameData;

        public void Init(IEcsSystems systems)
        {
            InitControls();
            AddPlayerAbilities();
            AddEnemiesAbilities();
        }

        private void InitControls()
        {
            foreach (var entity in _playerFilter.Value)
            {
                var player = _playerPool.Value.Get(entity);

                _gameData.Value.Input._Player.Ability01_Tap.started
                    += ctx => _abilityService.Value.TryActivateAbility(0, entity);
                _gameData.Value.Input._Player.Ability02_Tap.started
                    += ctx => _abilityService.Value.TryActivateAbility(1, entity);
                _gameData.Value.Input._Player.Ability03_Tap.started
                    += ctx => _abilityService.Value.TryActivateAbility(2, entity);
                _gameData.Value.Input._Player.Ability04_Tap.started
                    += ctx => _abilityService.Value.TryActivateAbility(3, entity);
                _gameData.Value.Input._Player.Action01_Tap.started
                    += ctx => _abilityService.Value.TryActivateAbility(4, entity);
                _gameData.Value.Input._Player.Action02_Tap.started
                    += ctx => _abilityService.Value.TryActivateAbility(5, entity);
                _gameData.Value.Input._Player.Action03_Tap.started
                    += ctx => _abilityService.Value.TryActivateAbility(6, entity);
                _gameData.Value.Input._Player.Action04_Tap.started
                    += ctx => _abilityService.Value.TryActivateAbility(7, entity);
            }
        }

        private void AddPlayerAbilities()
        {
            foreach (var entity in _playerFilter.Value)
            {
                var unit = _unitService.Value.GetUnit(entity);

                foreach (var abilityItem in unit.Config.Abilities)
                {
                    _abilityService.Value.CreateAbility(abilityItem, entity);

                    _canvasView.Value.AbilityImages[abilityItem.slotIndex].sprite = abilityItem.abilityConfig.icon;
                }
            }
        }

        private void AddEnemiesAbilities()
        {
            foreach (var entity in _enemyFilter.Value)
            {
                var unit = _unitService.Value.GetUnit(entity);
                
                foreach (var abilityItem in unit.Config.Abilities)
                    _abilityService.Value.CreateAbility(abilityItem, entity);
            }
        }
    }
}