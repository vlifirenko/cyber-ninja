using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
using CyberNinja.Services.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class PickupSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PickupComponent>> _filter;
        private EcsPoolInject<PickupComponent> _pickupPool;
        private EcsPoolInject<WeaponComponent> _weaponPool;
        private EcsPoolInject<TriggerComponent> _triggerPool;
        private EcsCustomInject<IUnitService> _unitService;
        
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
                    EquipWeapon(entity, pickup);
                }
            }
        }
        
        private void DropPrevWeapon(int unitEntity)
        {
            var prevWeapon = _weaponPool.Value.Get(unitEntity);
            var unit = _unitService.Value.GetUnit(unitEntity);
            var position = unit.View.Transform.position;
            
            prevWeapon.SceneView.gameObject.SetActive(true);
            prevWeapon.SceneView.Transform.position = position;
            _weaponPool.Value.Del(unitEntity);
            _triggerPool.Value.Get(unitEntity).Transforms.Add(prevWeapon.SceneView.Transform);
        }

        private void EquipWeapon(int unit, PickupComponent pickup)
        {
            _weaponPool.Value.Add(unit) = new WeaponComponent
            {
                Config = pickup.ItemConfig,
                SceneView = pickup.ItemSceneView
            };
            _triggerPool.Value.Get(unit).Transforms.Remove(pickup.ItemSceneView.Transform);
            pickup.ItemSceneView.gameObject.SetActive(false);
        }
    }
}