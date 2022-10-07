using System;
using CyberNinja.Config;
using CyberNinja.Events;
using CyberNinja.Models.Enums;
using CyberNinja.Services;
using CyberNinja.Services.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Item
{
    public class UseItemSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<IUnitService> _unitService;
        
        public void Init(IEcsSystems systems)
        {
            ItemEventsHolder.OnUseItem += (entity, effect) =>
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

        private void Heal(int entity, ItemUseEffect effect)
        {
            var health = _unitService.Value.GetHealth(entity);
            var newValue = Math.Clamp(health.Current + effect.value, 0f, health.Max);
            
            _unitService.Value.UpdateHealth(entity, newValue);
        }
    }
}