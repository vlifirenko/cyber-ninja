using System;
using CyberNinja.Models.Ability;
using CyberNinja.Models.Config;
using CyberNinja.Models.Enums;
using CyberNinja.Views.Core;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace CyberNinja.Views.Unit
{
    public class UnitView : AEntityView
    {
        [SerializeField] private UnitConfig config;
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform vfxSpawnPoint;
        [SerializeField] private UnitVfxView vfxView;
        [SerializeField] private UnitWeaponSlotView weaponSlotView;
        [Header("Enemy")]
        [SerializeField] private bool isFreeze;

        public UnitConfig Config => config;
        public Animator Animator => animator;
        public NavMeshAgent NavMeshAgent => navMeshAgent;
        public Transform VfxSpawnPoint => vfxSpawnPoint;
        public UnitVfxView VfxView => vfxView;
        public UnitWeaponSlotView WeaponSlotView => weaponSlotView;

        public bool IsFreeze => isFreeze;
    }
}