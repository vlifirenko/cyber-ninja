using System.Linq;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Enums;
using CyberNinja.Services;
using CyberNinja.Services.Unit;
using CyberNinja.Utils;
using CyberNinja.Views;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class InitUnitsSystem : IEcsInitSystem
    {
        private readonly EcsWorldInject _world;
        private readonly EcsCustomInject<SceneView> _sceneView;
        private readonly EcsCustomInject<IAiService> _aiService;
        private readonly EcsCustomInject<IUnitService> _unitService;

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
                    Debug.Log(entity);
                }
                else
                {
                    var enemyPool = _world.Value.GetPool<EnemyComponent>();
                    
                    enemyPool.Add(entity);
                    _aiService.Value.InitUnit(entity);
                }
            }
        }
    }
}