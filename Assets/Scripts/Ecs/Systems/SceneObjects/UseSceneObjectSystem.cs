using System;
using CyberNinja.Events;
using CyberNinja.Models;
using CyberNinja.Models.Enums;
using CyberNinja.Services.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.SceneObjects
{
    public class UseSceneObjectSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<IUnitService> _unitService;
        
        public void Init(IEcsSystems systems)
        {
            SceneEventsHolder.OnUseSceneObject += (entity, effect) =>
            {
                switch (effect.type)
                {
                    case EItemUseEffectType.Heal:
                        Heal(entity, effect);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(effect), effect, null);
                }
            };
        }

        private void Heal(int entity, SceneObjectUseEffect effect)
        {
            var health = _unitService.Value.GetHealth(entity);
            var newValue = Math.Clamp(health.Current + effect.value, 0f, health.Max);
            
            _unitService.Value.UpdateHealth(entity, newValue);
        }
    }
}