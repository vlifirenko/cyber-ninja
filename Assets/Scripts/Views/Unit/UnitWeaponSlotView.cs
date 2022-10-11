using CyberNinja.Views.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace CyberNinja.Views.Unit
{
    public class UnitWeaponSlotView : AView
    {
        [FoldoutGroup("REFERENCES"), SerializeField] private Transform weaponsContainer;
        [FoldoutGroup("REFERENCES"), SerializeField] private Transform handLeft;
        [FoldoutGroup("REFERENCES"), SerializeField] private Transform handRight;
        [FoldoutGroup("REFERENCES"), SerializeField] private bool useRightHand = true;

        [ToggleGroup("WEAPONS"), SerializeField] private bool WEAPONS;
        [FormerlySerializedAs("weaponInstance")] [VerticalGroup("WEAPONS/1"), SerializeField, ReadOnly] private ItemView itemInstance;

        public Transform WeaponsContainer => weaponsContainer;
        public Transform HandLeft => handLeft;
        public Transform HandRight => handRight;
        public bool UseRightHand => useRightHand;
        public bool IsWeaponEnabled => WEAPONS;

        public ItemView ItemInstance
        {
            get => itemInstance;
            set => itemInstance = value;
        }
    }
}