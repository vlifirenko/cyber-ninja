using CyberNinja.Views.Core;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

namespace CyberNinja.Views.Unit
{
    public class UnitVfxView : AView
    {
        [FoldoutGroup("References"), SerializeField] private Animator anim;
        
        [FoldoutGroup("Footsteps"), SerializeField] private VisualEffect vfxFootsteps;
        [FoldoutGroup("Footsteps"), SerializeField] private StudioEventEmitter fmodFootsteps;

        private float? _speedMoveMax;
        private static readonly int hashSpeed = Animator.StringToHash("speed");

        public float? SpeedMoveMax
        {
            get => _speedMoveMax;
            set => _speedMoveMax = value;
        }

        private void Update() => FootstepsLogic();

        private void FootstepsLogic()
        {
            if (anim.GetFloat("speed") > 0.5f)
                EffectPlay(vfxFootsteps);

            if (_speedMoveMax.HasValue)
                fmodFootsteps.SetParameter("VolumeTrack", anim.GetFloat(hashSpeed) / _speedMoveMax.Value * 100);
        }

        private void EventAudioPlayFootsteps() => AudioPlay(fmodFootsteps);

        private static void AudioPlay(StudioEventEmitter obj) => obj.Play();

        private static void EffectPlay(VisualEffect obj) => obj.SendEvent("OnPlay");

        private void EffectStop(VisualEffect obj) => obj.SendEvent("OnStop");
    }
}