using CyberNinja.Models.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CyberNinja.Config
{
    [CreateAssetMenu(menuName = "Config/Ability", fileName = "Ability")]
    public class AbilityConfig : ScriptableObject
    {
        [FoldoutGroup("BASE", Expanded = true)]
        [HorizontalGroup("BASE/1", .7f, LabelWidth = 80)]
        [VerticalGroup("BASE/1/1")] public string abilityName;
        [VerticalGroup("BASE/1/1"), TextArea(4, 20)] public string description;
        [EnumToggleButtons]
        [VerticalGroup("BASE/1/1")] public EAbilityType abilityType;
        [EnumToggleButtons]
        [VerticalGroup("BASE/1/1")] public EActionType actionType;
        [Space]
        [PreviewField(110, ObjectFieldAlignment.Center)]
        [VerticalGroup("BASE/1/2"), HideLabel] public Sprite icon;
        
        [FoldoutGroup("PARAMETERS", Expanded = true)]
        [ToggleGroup("PARAMETERS/ATTRIBUTES", CollapseOthersOnExpand = false)] public bool ATTRIBUTES;
        [VerticalGroup("PARAMETERS/ATTRIBUTES/1")] public float damage;
        [VerticalGroup("PARAMETERS/ATTRIBUTES/1")] public float energyCost;
        
        [ToggleGroup("PARAMETERS/TIME", CollapseOthersOnExpand = false)] public bool TIME;
        [VerticalGroup("PARAMETERS/TIME/1")] public float charge; 
        [VerticalGroup("PARAMETERS/TIME/1")] public float duration;
        [VerticalGroup("PARAMETERS/TIME/1")] public float cooldown;

        [ToggleGroup("PARAMETERS/ANIMATOR", CollapseOthersOnExpand = false)] public bool ANIMATOR;                 
        [EnumPaging]
        [VerticalGroup("PARAMETERS/ANIMATOR/1")] public EAnimatorParameter animParameter;                          
        [VerticalGroup("PARAMETERS/ANIMATOR/1"), ToggleLeft] public bool animTrigger;                             
        [HorizontalGroup("PARAMETERS/ANIMATOR/2", .4f, LabelWidth = 1), ToggleLeft] public bool animUseBool;            
        [HorizontalGroup("PARAMETERS/ANIMATOR/2"), ToggleLeft] public bool animBoolValue;                              
        [HorizontalGroup("PARAMETERS/ANIMATOR/3", .4f, LabelWidth = 1), ToggleLeft] public bool animUseUpperBodyWeight; 
        [HorizontalGroup("PARAMETERS/ANIMATOR/3")] public float animUpperBodyWeightTime = .5f;

        [ToggleGroup("PARAMETERS/VFX", CollapseOthersOnExpand = false)] public bool VFX; 
        [VerticalGroup("PARAMETERS/VFX/1")] public GameObject vfxGameobject;             
        [Space]
        [VerticalGroup("PARAMETERS/VFX/1")] public float vfxSpawnDelay;                  
        [VerticalGroup("PARAMETERS/VFX/1")] public float vfxLifeTime;                    
        [Space]
        [VerticalGroup("PARAMETERS/VFX/1")] public Vector3 vfxPosition;                  
        [VerticalGroup("PARAMETERS/VFX/1")] public Vector3 vfxRotation;                  
        [Space]
        [VerticalGroup("PARAMETERS/VFX/1"), ToggleLeft] public bool vfxPlaceOutside;

        [ToggleGroup("PARAMETERS/UseLookVector", CollapseOthersOnExpand = false)] public bool UseLookVector;

        [ToggleGroup("PARAMETERS/SHAPE", CollapseOthersOnExpand = false)] public bool SHAPE;
        [HorizontalGroup("PARAMETERS/SHAPE/1", .4f, LabelWidth = 1), ToggleLeft] public bool useRadius;
        [HorizontalGroup("PARAMETERS/SHAPE/1")] public float radius;                                     
        [HorizontalGroup("PARAMETERS/SHAPE/2", .4f, LabelWidth = 1), ToggleLeft] public bool useAngle;
        [HorizontalGroup("PARAMETERS/SHAPE/2")] public float angle;

        [ToggleGroup("PARAMETERS/STATIONARY", CollapseOthersOnExpand = false), ToggleLeft] public bool STATIONARY;
        [VerticalGroup("PARAMETERS/STATIONARY/1")] public float stationaryTime;

        [ToggleGroup("PARAMETERS/STUN", CollapseOthersOnExpand = false)] public bool STUN;    
        [VerticalGroup("PARAMETERS/STUN/1")] public float stunTime;                            
        
        [ToggleGroup("PARAMETERS/DASH", CollapseOthersOnExpand = false)] public bool DASH;     
        [VerticalGroup("PARAMETERS/DASH/1")] public float dashTime;                           
        [VerticalGroup("PARAMETERS/DASH/1")] public float dashDistance;                       
        [VerticalGroup("PARAMETERS/DASH/1")] public bool dashUseLook;                         

        [FoldoutGroup("DEFENCE", Expanded = true)]
        [ToggleGroup("DEFENCE/SHIELD", CollapseOthersOnExpand = false)] public bool SHIELD;
        [Space]
        [VerticalGroup("DEFENCE/SHIELD/1")] public float shieldDamagePhysicalFactor;      
        [VerticalGroup("DEFENCE/SHIELD/1")] public float shieldDamagePhysicalTime;         

        [FoldoutGroup("FLAGS", Expanded = true)]
        [VerticalGroup("FLAGS/1")] [ToggleLeft] public bool inputBlockable;
    }
}