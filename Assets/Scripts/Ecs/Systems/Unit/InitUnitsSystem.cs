using System.Linq;
using CyberNinja.Ecs.Components.Unit;
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

namespace CyberNinja.Ecs.Systems.Unit
{
    public class InitUnitsSystem : IEcsInitSystem
    {
        private readonly EcsWorldInject _world;
        private readonly EcsCustomInject<SceneView> _sceneView;
        private readonly EcsCustomInject<IAiService> _aiService;
        private readonly EcsCustomInject<IUnitService> _unitService;

        [EcsUguiNamed(UiConst.HealthSliderContainer)]
        private UiHealthSliderContainer _healthSliderContainer;

        public void Init(IEcsSystems systems)
        {
            foreach (var view in _sceneView.Value.UnitContainer.Items)
            {
                var entity = _unitService.Value.CreateUnit(view);

                if (view.CompareTag(Tag.Player))
                {
                    var playerPool = _world.Value.GetPool<PlayerComponent>();
                    playerPool.Add(entity);

                    _unitService.Value.Player = _world.Value.PackEntity(entity);
                }
                else
                {
                    var enemyPool = _world.Value.GetPool<EnemyComponent>();
                    var enemy = new EnemyComponent();
                    var instance = Object.Instantiate(_healthSliderContainer.Prefab, _healthSliderContainer.Container);

                    enemy.HealthSlider = instance;
                    instance.gameObject.SetActive(true);
                    
                    enemyPool.Add(entity) = enemy;
                    _aiService.Value.InitUnit(entity);
                }
            }
        }
    }
}