using CyberNinja.Views.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CyberNinja.Views.Unit
{
    public class UnitWeaponSlotView : AView
    {
        [FoldoutGroup("REFERENCES"), SerializeField] private Transform weaponsContainer;
        [FoldoutGroup("REFERENCES"), SerializeField] private Transform handLeft;
        [FoldoutGroup("REFERENCES"), SerializeField] private Transform handRight;
        [FoldoutGroup("REFERENCES"), SerializeField] private bool useRightHand = true;

        [ToggleGroup("WEAPONS"), SerializeField] private bool WEAPONS;
        [VerticalGroup("WEAPONS/1"), SerializeField] private WeaponView weaponPack;
        [VerticalGroup("WEAPONS/1"), SerializeField, ReadOnly] private GameObject weaponInstance;

        public Transform WeaponsContainer => weaponsContainer;
        public Transform HandLeft => handLeft;
        public Transform HandRight => handRight;
        public bool UseRightHand => useRightHand;
        public bool IsWeaponEnabled => WEAPONS;
        public WeaponView WeaponPack => weaponPack;
        public GameObject WeaponInstance => weaponInstance;
    }
}