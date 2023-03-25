using CyberNinja.Models.Ability;
using CyberNinja.Models.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Unit", fileName = "Unit")]
    public class UnitConfig : ScriptableObject
    {
        [FoldoutGroup("CONTROL"), SerializeField] private EControlType controlType;
        [Space]
        [FoldoutGroup("CONTROL/AI"), SerializeField] private float lookRadius = 8f;
        [FoldoutGroup("CONTROL/AI"), SerializeField] private float maxChaseDistance = 12f;
        [Space]
        [FoldoutGroup("CONTROL/AI"), SerializeField] private float attackDistance = 2.5f;
        [FoldoutGroup("CONTROL/AI"), SerializeField] private float maxAttackDistance = 3.5f;
        
        [FoldoutGroup("BASIC"), SerializeField] private float maxHealth = 100;
        [FoldoutGroup("BASIC"), SerializeField] private float maxEnergy = 100;
        [Space]
        [FoldoutGroup("BASIC"), SerializeField] private float healthRegeneration;
        [FoldoutGroup("BASIC"), SerializeField] private float energyRegeneration;
        
        [FoldoutGroup("MOVE"), SerializeField] private float moveSpeed = 10f;
        [FoldoutGroup("MOVE"), SerializeField] private float speedFactor = 1f;
        [Space]
        [FoldoutGroup("MOVE"), SerializeField] private float smoothMove = .1f;
        [FoldoutGroup("MOVE"), SerializeField] private float smoothRotation = .4f;
        [FoldoutGroup("MOVE"), SerializeField] private float abilitySmoothRotation = 1;
        
        [FoldoutGroup("VECTORS"), SerializeField] [ToggleLeft] private bool rotateToLookVector;
        [FoldoutGroup("VECTORS"), SerializeField] [Range(0.01f, 1)] private float diffVectorLerp = 0.12f;
        [Space]
        [FoldoutGroup("VECTORS"), Range(0.1f, 5)] [SerializeField] private float minMouseMagnitude = 1.5f;
        [FoldoutGroup("VECTORS"), Range(0.1f, 1)] [SerializeField] private float mouseMagnitudeOffset = 0.2f;
        [FoldoutGroup("VECTORS"), SerializeField] [ReadOnly] private float minMouseMagnitudeTemp;
        
        [FoldoutGroup("ABILITIES"), SerializeField] private AbilityItem[] abilities;
        [Space]
        [FoldoutGroup("ABILITIES"), SerializeField] private AbilityConfig abilityDamageConfig;
        [FoldoutGroup("ABILITIES")] [SerializeField] private AbilityConfig abilityStunOnConfig;
        [FoldoutGroup("ABILITIES")] [SerializeField] private AbilityConfig abilityStunOffConfig;
        [FoldoutGroup("ABILITIES")] [SerializeField] private AbilityConfig abilityDeadConfig;
        [FoldoutGroup("ABILITIES")] [SerializeField] private AbilityConfig abilityReviveConfig;

        [SerializeField] private ItemConfig defaultWeapon;
        public bool isHasDroid;
        
        public EControlType ControlType => controlType;
        public float LookRadius => lookRadius;
        public float MaxChaseDistance => maxChaseDistance;
        public float AttackDistance => attackDistance;
        public float MaxAttackDistance => maxAttackDistance;
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float MaxEnergy { get => maxEnergy; set => maxEnergy = value; }
        public float HealthRegeneration => healthRegeneration;
        public float EnergyRegeneration => energyRegeneration;
        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public float SpeedFactor { get => speedFactor; set => speedFactor = value; }
        public float SmoothMove { get => smoothMove; set => smoothMove = value; }
        public float SmoothRotation { get => smoothRotation; set => smoothRotation = value; }
        public float AbilitySmoothRotation { get => abilitySmoothRotation; set => abilitySmoothRotation = value; }
        public bool RotateToLookVector => rotateToLookVector;
        public float DiffVectorLerp => diffVectorLerp;
        public float MinMouseMagnitude => minMouseMagnitude;
        public float MouseMagnitudeOffset => mouseMagnitudeOffset;
        public float MinMouseMagnitudeTemp { get => minMouseMagnitudeTemp; set => minMouseMagnitudeTemp = value; }
        public AbilityItem[] Abilities => abilities;
        public AbilityConfig AbilityDamageConfig => abilityDamageConfig;
        public AbilityConfig AbilityStunOnConfig => abilityStunOnConfig;
        public AbilityConfig AbilityStunOffConfig => abilityStunOffConfig;
        public AbilityConfig AbilityDeadConfig => abilityDeadConfig;
        public AbilityConfig AbilityReviveConfig => abilityReviveConfig;
        public ItemConfig DefaultWeapon => defaultWeapon;
    }
}