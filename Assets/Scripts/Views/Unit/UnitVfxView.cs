using CyberNinja.Utils;
using CyberNinja.Views.Core;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace CyberNinja.Views.Unit
{
    public class UnitVfxView : AView
    {
        [FoldoutGroup("References"), SerializeField] private Animator animator;

        [FoldoutGroup("Footsteps"), SerializeField] private VisualEffect vfxFootsteps;
        [FoldoutGroup("Footsteps"), SerializeField] private StudioEventEmitter fmodFootsteps;

        private float? _speedMoveMax;

        public float? SpeedMoveMax
        {
            get => _speedMoveMax;
            set => _speedMoveMax = value;
        }

        private void Update() => FootstepsLogic();

        private void FootstepsLogic()
        {
            if (animator.GetFloat(AnimatorHash.Speed) > 0.5f)
                EffectPlay(vfxFootsteps);

            if (_speedMoveMax.HasValue)
                fmodFootsteps.SetParameter("VolumeTrack", animator.GetFloat(AnimatorHash.Speed) / _speedMoveMax.Value * 100);
        }

        private static void EffectPlay(VisualEffect obj) => obj.SendEvent("OnPlay");

        private void EffectStop(VisualEffect obj) => obj.SendEvent("OnStop");
    }
}