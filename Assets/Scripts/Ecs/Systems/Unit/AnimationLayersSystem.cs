using CyberNinja.Config;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class AnimationLayersSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<LayerUpperBodyComponent>> _filter;
        private readonly EcsPoolInject<LayerUpperBodyTimeComponent> _layerUpperBodyTimePool;
        private readonly EcsPoolInject<LayerUpperBodyComponent> _layerUpperBodyPool;
        private readonly EcsCustomInject<UnitConfig> _characterConfig;
        private readonly EcsCustomInject<IUnitService> _unitService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var unit = _unitService.Value.GetUnit(entity);
                var animator = unit.View.Animator;

                var upperBodyLayerWeight = animator.GetLayerWeight(1);
                var turnOnLayer = true;

                if (_layerUpperBodyTimePool.Value.Has(entity))
                {
                    ref var time = ref _layerUpperBodyTimePool.Value.Get(entity).Value;
                    time -= Time.deltaTime;

                    if (time <= 0)
                        _layerUpperBodyTimePool.Value.Del(entity);

                    if (upperBodyLayerWeight > 1 - _characterConfig.Value.layerWeightTreshold)
                        animator.SetLayerWeight(1, 1);
                    else
                        animator.SetLayerWeight(1,
                            Mathf.Lerp(upperBodyLayerWeight, 1, _characterConfig.Value.layerWeightLerp));
                }
                else
                {
                    if (upperBodyLayerWeight < _characterConfig.Value.layerWeightTreshold)
                    {
                        animator.SetLayerWeight(1, 0);
                        turnOnLayer = false;
                    }
                    else
                        animator.SetLayerWeight(1,
                            Mathf.Lerp(upperBodyLayerWeight, 0, _characterConfig.Value.layerWeightLerp));
                }

                if (!turnOnLayer)
                    _layerUpperBodyPool.Value.Del(entity);
            }
        }
    }
}