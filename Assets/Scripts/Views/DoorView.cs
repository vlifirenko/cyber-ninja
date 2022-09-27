using CyberNinja.Views.Core;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CyberNinja.Views
{
    public class DoorView : AEntityView
    {
        [BoxGroup("CONFIGS"), SerializeField] private Transform targetDoor;
        [Space]
        [BoxGroup("CONFIGS"), SerializeField] private Transform outPosition;
        [Space]
        [ToggleGroup("CONFIGS/PARAMETERS", CollapseOthersOnExpand = false), SerializeField] private bool PARAMETERS;
        [VerticalGroup("CONFIGS/PARAMETERS/1"), SerializeField] private float delay;
        [VerticalGroup("CONFIGS/PARAMETERS/1"), SerializeField] private float cooldown;  
        [Space]
        [ToggleGroup("CONFIGS/AUTOMATIC", CollapseOthersOnExpand = false), SerializeField] private bool AUTOMATIC;
        [VerticalGroup("CONFIGS/AUTOMATIC/1"), SerializeField] private float automaticDelay;
        [VerticalGroup("CONFIGS/AUTOMATIC/1"), SerializeField] private float automaticCooldown;
        [Space]
        [ToggleGroup("CONFIGS/VFX", CollapseOthersOnExpand = false), SerializeField] private bool VFX;
        [VerticalGroup("CONFIGS/VFX/1"), SerializeField] private GameObject vfxActivationPrefab;
        [VerticalGroup("CONFIGS/VFX/1"), SerializeField] private float vfxSpawnDelay;
        [VerticalGroup("CONFIGS/VFX/1"), SerializeField] private float vfxLifeTime;
        
        public Transform TargetDoor => targetDoor;
        public Transform OutPosition => outPosition;
        public bool Parameters => PARAMETERS;
        public float Delay => delay;
        public float Cooldown => cooldown;
        public bool Automatic => AUTOMATIC;
        public float AutomaticDelay => automaticDelay;
        public float AutomaticCooldown => automaticCooldown;
        public bool Vfx => VFX;
        public GameObject VfxActivationPrefab => vfxActivationPrefab;
        public float VfxSpawnDelay => vfxSpawnDelay;
        public float VfxLifeTime => vfxLifeTime;
    }
}