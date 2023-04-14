﻿using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
using CyberNinja.Services;
using CyberNinja.Services.Unit;
using CyberNinja.Utils;
using CyberNinja.Views;
using CyberNinja.Views.Ui;
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
        private readonly EcsCustomInject<UnitService> _unitService;
        private EcsCustomInject<GlobalUnitConfig> _globalUnitConfig;

        [EcsUguiNamed(UiConst.HealthSliderContainer)]
        private UiHealthSliderContainer _healthSliderContainer;

        public void Init(IEcsSystems systems)
        {
            foreach (var view in _sceneView.Value.UnitContainer.Items)
            {
                var entity = _unitService.Value.CreateUnit(view);

                if (view.CompareTag(Tag.Player))
                {
                    
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