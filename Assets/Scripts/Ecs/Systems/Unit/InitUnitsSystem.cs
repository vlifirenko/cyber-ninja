using System.Linq;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Enums;
using CyberNinja.Services;
using CyberNinja.Views;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class InitUnitsSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<SceneView> _sceneView;
        private readonly EcsCustomInject<IAiService> _aiService;
        private readonly EcsCustomInject<IUnitService> _unitService;
        private readonly EcsPoolInject<EnemyComponent> _enemyPool;

        public void Init(IEcsSystems systems)
        {
            InitPlayer();
            InitEnemies();
        }

        private void InitPlayer()
        {
            var view = _sceneView.Value.PlayerView;
            var entity = _unitService.Value.CreateUnit(view);

            var playerPool = _enemyPool.Value.GetWorld().GetPool<PlayerComponent>();
            playerPool.Add(entity);
        }

        private void InitEnemies()
        {
            var enemies = _sceneView.Value.UnitContainerView.Items.ToList();

            enemies.RemoveAll(gameObject => gameObject.CompareTag(ETags.Player.ToString()));
            if (enemies.Count == 0)
                return;

            foreach (var item in enemies)
            {
                var view = item.GetComponent<UnitView>();
                var entity = _unitService.Value.CreateUnit(view);

                _enemyPool.Value.Add(entity);
                _aiService.Value.InitUnit(entity);
            }
        }
    }
}