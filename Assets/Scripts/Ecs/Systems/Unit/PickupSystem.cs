using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
using CyberNinja.Services;
using CyberNinja.Services.Unit;
using CyberNinja.Utils;
using CyberNinja.Views.Ui;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class PickupSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PickupComponent>> _filter;
        private EcsPoolInject<PickupComponent> _pickupPool;
        private EcsPoolInject<WeaponComponent> _weaponPool;
        private EcsPoolInject<TriggerComponent> _triggerPool;
        private EcsCustomInject<IUnitService> _unitService;
        private EcsCustomInject<IItemService> _itemService;
        private EcsWorldInject _world;

        [EcsUguiNamed(UiConst.ItemPopup)] private UiItemPopup _uiItemPopup;

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
                    else
                        _uiItemPopup.Hide();

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

            int? weaponEntity;
            if (!pickup.ItemSceneView.Entity.HasValue)
            {
                weaponEntity = _itemService.Value.CreateItem(pickup.ItemConfig);
                pickup.ItemSceneView.Entity = weaponEntity;
            }
            else
                weaponEntity = pickup.ItemSceneView.Entity;

            _itemService.Value.TryEquip(weaponEntity.Value, _world.Value.PackEntityWithWorld(unit));
        }
    }
}