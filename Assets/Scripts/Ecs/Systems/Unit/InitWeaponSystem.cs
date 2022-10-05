using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class InitWeaponSystem : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<UnitComponent>> _filter;
        private readonly EcsPoolInject<UnitComponent> _unitPool;

        public void Init(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var unit = _unitPool.Value.Get(entity);
                var weaponView = unit.View.WeaponSlotView;

                if (!weaponView.IsWeaponEnabled)
                    return;

                if (weaponView.WeaponInstance != null)
                    Object.Destroy(weaponView.WeaponInstance.gameObject);

                var hand = weaponView.UseRightHand ? weaponView.HandRight : weaponView.HandLeft;
                var weaponInstance = Object.Instantiate(weaponView.WeaponPack,
                    hand, false);

                weaponInstance.Transform.localPosition = weaponInstance.Position;
                weaponInstance.Transform.localRotation = Quaternion.Euler(weaponInstance.Rotation);
                weaponInstance.Transform.localScale = weaponInstance.Scale;

                weaponView.WeaponInstance = weaponInstance;
            }
        }

        private void DeleteWeapons()
        {
            foreach (var entity in _filter.Value)
            {
                var unit = _unitPool.Value.Get(entity);
                var weaponView = unit.View.WeaponSlotView;

                for (var i = 0; i < weaponView.WeaponsContainer.childCount; i++)
                {
                    Object.DestroyImmediate(weaponView.WeaponsContainer
                        .GetChild(weaponView.WeaponsContainer.childCount - i - 1).gameObject);
                }
            }
        }
    }
}