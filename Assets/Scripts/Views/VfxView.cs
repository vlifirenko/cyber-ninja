using System.Collections;
using CyberNinja.Views.Core;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

namespace CyberNinja.Views
{
    public class VfxView : AView
    {
        [FoldoutGroup("REFERENCES"), SerializeField] private VisualEffect effect;
        [FoldoutGroup("REFERENCES"), SerializeField] private StudioEventEmitter fmod;

        [FoldoutGroup("PARAMETERS"), GUIColor(.6f, .6f, 1f), OnValueChanged("UpdateVfxParameters"), Range(0, 100), SerializeField] private float vfxLifetime;
        [FoldoutGroup("PARAMETERS"), ToggleLeft, SerializeField] private bool playOnStart;
        [FoldoutGroup("PARAMETERS"), ToggleLeft, SerializeField] private bool parametersUpdate;
        [FoldoutGroup("PARAMETERS"), SerializeField] private bool frameCheck = true;

        [ToggleGroup("PARAMETERS/AUDIO_Activate", CollapseOthersOnExpand = false), SerializeField] private bool AUDIO_Activate;
        [VerticalGroup("PARAMETERS/AUDIO_Activate/1"), OnValueChanged("UpdateVfxParameters"), SerializeField, Range(0, 100)] private int audioVolume = 100;
        [VerticalGroup("PARAMETERS/AUDIO_Activate/1"), Range(0, 10), SerializeField] private float audioDelay;
        [VerticalGroup("PARAMETERS/AUDIO_Activate/1"), ToggleLeft, SerializeField] private bool audioPhaseUpdate;

        [ToggleGroup("PARAMETERS/EFFECT_Activate", CollapseOthersOnExpand = false), SerializeField] private bool EFFECT_Activate;
        [VerticalGroup("PARAMETERS/EFFECT_Activate/1"), GUIColor(.6f, .6f, 1f), OnValueChanged("UpdateVfxParameters"), Range(0, 100), SerializeField] private float effectRadius = 1;
        [VerticalGroup("PARAMETERS/EFFECT_Activate/1"), Range(0, 10)] private float effectDelay;
        
        [ToggleGroup("PARAMETERS/HIT_Activate", CollapseOthersOnExpand = false), SerializeField] private bool HIT_Activate;
        [VerticalGroup("PARAMETERS/HIT_Activate/1"), GUIColor(.6f, .6f, 1f), OnValueChanged("UpdateVfxParameters"), Range(0, 1), SerializeField] private float vfxDamage;
        [VerticalGroup("PARAMETERS/HIT_Activate/1"), GUIColor(.6f, .6f, 1f), OnValueChanged("UpdateVfxParameters"), Range(0, 1), SerializeField] private float vfxBlood;

        [FoldoutGroup("TRANSFORM"), ToggleLeft, SerializeField] private bool vfxGlobalRotation;
        
        private bool _activatePhase;
        private float _currentPhaseTime;

        public float VfxLifetime
        {
            set => vfxLifetime = value;
        }
        public float EffectRadius
        {
            set => effectRadius = value;
        }
        public float VfxDamage
        {
            set => vfxDamage = value;
        }
        public float VfxBlood
        {
            set => vfxBlood = value;
        }

        public void Start()
        {
            if (!playOnStart)
                return;
            
            RunVFX();
        }

        private void Update()
        {
            if (frameCheck)
            {
                frameCheck = false;
                UpdateVfxParameters();
            }

            if (parametersUpdate)
                UpdateVfxParameters();
            if (audioPhaseUpdate)
                PhaseMath();

            VfxGlobalRotation();
        }

        private IEnumerator PlayEffect(float delay)
        {
            yield return new WaitForSeconds(delay);
            effect.SendEvent("OnPlay");
        }

        private IEnumerator PlayAudio(float delay)
        {
            yield return new WaitForSeconds(delay);
            fmod.Play();
            if (audioPhaseUpdate)
            {
                _activatePhase = true;
                _currentPhaseTime = 0;
            }
        }

        private void PhaseMath()
        {
            if (!_activatePhase)
                return;

            _currentPhaseTime += Time.deltaTime;
            var finalValue = _currentPhaseTime / vfxLifetime;
            if (_currentPhaseTime >= vfxLifetime) finalValue = 1;
            fmod.SetParameter("phase", finalValue);
        }
        
        [ButtonGroup("BUTTONS/FLAGS")]
        private void RunVFX()
        {
            if (EFFECT_Activate)
                StartCoroutine(PlayEffect(effectDelay));
            if (AUDIO_Activate)
                StartCoroutine(PlayAudio(effectDelay));
        }

        [FoldoutGroup("BUTTONS")]
        [ButtonGroup("BUTTONS/FLAGS")]
        private void UpdateVfxParameters()
        {
            if (AUDIO_Activate)
            {
                fmod.SetParameter("VolumeMaster", audioVolume);
                fmod.SetParameter("Damage", vfxDamage);
                fmod.SetParameter("Blood", vfxBlood);
            }
        
            if (EFFECT_Activate)
            {
                if (effect.HasFloat("Lifetime")) effect.SetFloat("Lifetime", vfxLifetime);
                if (effect.HasFloat("Radius")) effect.SetFloat("Radius", effectRadius);
            }
        
            if (HIT_Activate)
            {
                if (effect.HasFloat("Damage")) effect.SetFloat("Damage", vfxDamage);
                if (effect.HasFloat("Blood")) effect.SetFloat("Blood", vfxBlood);
            }
        }

        private void VfxGlobalRotation()
        {
            if (!vfxGlobalRotation)
                return;

            transform.rotation = Quaternion.identity;
        }
    }
}