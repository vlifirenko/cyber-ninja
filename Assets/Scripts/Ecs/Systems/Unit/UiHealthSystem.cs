using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Services;
using CyberNinja.Utils;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class UiHealthSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UiHealthSliderComponent>, Exc<DeadComponent>> _filter;
        private readonly EcsCustomInject<CanvasView> _canvasView;
        private readonly EcsCustomInject<IUnitService> _unitService;
        private readonly EcsPoolInject<UiHealthSliderComponent> _healthSliderPool;
        private readonly EcsPoolInject<HealthComponent> _healthPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var healthSlider = _healthSliderPool.Value.Get(entity);
                var unit = _unitService.Value.GetUnit(entity);
                var health = _healthPool.Value.Get(entity);
                var position = _canvasView.Value.Canvas.WorldToCanvasPosition(unit.View.Transform.position);
                
                healthSlider.Value.GetComponent<RectTransform>().anchoredPosition = position + new Vector3(0, 160, 0);
                healthSlider.Value.value = health.Current / health.Max;
            }
        }
    }
}