using CyberNinja.Ecs.Components.Room;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
using CyberNinja.Services;
using CyberNinja.Services.Unit;
using CyberNinja.Utils;
using CyberNinja.Views;
using CyberNinja.Views.Ui;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using Object = UnityEngine.Object;

namespace CyberNinja.Ecs.Systems.Room
{
    public class UpdateRoomSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<UpdateRoomComponent>> _filter;
        private EcsPoolInject<UpdateRoomComponent> _updateRoomPool;
        private readonly EcsWorldInject _world;
        private readonly EcsCustomInject<SceneView> _sceneView;
        private readonly EcsCustomInject<IAiService> _aiService;
        private readonly EcsCustomInject<UnitService> _unitService;
        private EcsCustomInject<GlobalUnitConfig> _globalUnitConfig;

        [EcsUguiNamed(UiConst.HealthSliderContainer)]
        private UiHealthSliderContainer _healthSliderContainer;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var updateRoom = _updateRoomPool.Value.Get(entity);
                var room = updateRoom.Room;

                SpawnEnemies(room.Enemies);
            }
        }

        public void SpawnEnemies(UnitView[] list)
        {
            foreach (var view in list)
            {
                var entity = _unitService.Value.CreateUnit(view);
                var enemyPool = _world.Value.GetPool<EnemyComponent>();
                var enemy = new EnemyComponent();
                var instance = Object.Instantiate(_healthSliderContainer.Prefab, _healthSliderContainer.Container);

                enemy.HealthSlider = instance;
                instance.gameObject.SetActive(true);

                enemyPool.Add(entity) = enemy;
                _aiService.Value.InitUnit(entity);
                
                view.Show();
            }
        }
    }
}