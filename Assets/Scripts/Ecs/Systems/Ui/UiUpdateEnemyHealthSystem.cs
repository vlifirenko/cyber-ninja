using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Utils;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Ui
{
    public class UiUpdateEnemyHealthSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<HealthComponent, EnemyComponent>, Exc<DeadComponent>> _filter;
        private EcsPoolInject<HealthComponent> _healthPool;
        private EcsPoolInject<EnemyComponent> _enemyPool;
        private EcsPoolInject<UnitComponent> _unitComponent;
        private EcsCustomInject<CanvasView> _canvasView;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var health = _healthPool.Value.Get(entity);
                var enemy = _enemyPool.Value.Get(entity);
                var unit = _unitComponent.Value.Get(entity);

                var uiView = enemy.HealthSlider;
                var position = _canvasView.Value.Canvas.WorldToCanvasPosition(unit.View.Transform.position);

                position.x += uiView.Offset.x;
                position.y += uiView.Offset.y;

                uiView.GetComponent<RectTransform>().anchoredPosition = position;
                uiView.Slider.value = health.Current / health.Max;
            }
        }
    }
}