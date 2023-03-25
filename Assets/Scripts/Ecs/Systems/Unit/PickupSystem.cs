using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class PickupSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PickupComponent>> _filter;
        private EcsPoolInject<PickupComponent> _pickupPool;
        private EcsPoolInject<WeaponComponent> _weaponPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var pickup = _pickupPool.Value.Get(entity);
                var config = pickup.ItemConfig;
                
                if (config.type == EItemType.Weapon)
                {
                    if (_weaponPool.Value.Has(entity))
                        DropPrevWeapon(entity);
                    EquipWeapon(entity, config);
                }
            }
        }
        
        private void DropPrevWeapon(int unit)
        {
            // todo drop weapon
            _weaponPool.Value.Del(unit);
        }

        private void EquipWeapon(int unit, ItemConfig config)
        {
            _weaponPool.Value.Add(unit) = new WeaponComponent
            {
                Config = config
            };
        }
    }
}