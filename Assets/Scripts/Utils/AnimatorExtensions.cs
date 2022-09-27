using System;
using CyberNinja.Config;
using CyberNinja.Models.Ability;
using CyberNinja.Models.Enums;
using UnityEngine;

namespace CyberNinja.Utils
{
    public static class AnimatorExtensions
    {
        private static readonly int baseTrigger = Animator.StringToHash("_baseTrigger");
        private static readonly int actionTrigger = Animator.StringToHash("_actionTrigger");
        private static readonly int skillTrigger = Animator.StringToHash("_skillTrigger");

        public static void TriggerAnimations(this Animator animator, AbilityConfig abilityConfig)
        {
            if (abilityConfig.animTrigger)
            {
                switch (abilityConfig.abilityType)
                {
                    case EAbilityType.Empty:
                        // empty
                        break;
                    case EAbilityType.Layer:
                        // empty
                        break;
                    case EAbilityType.Base:
                        animator.SetTrigger(baseTrigger);
                        break;
                    case EAbilityType.Action:
                        animator.SetTrigger(actionTrigger);
                        break;
                    case EAbilityType.Skill:
                        animator.SetTrigger(skillTrigger);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (abilityConfig.animUseBool)
                animator.SetBool(abilityConfig.animParameter.ToString(), abilityConfig.animBoolValue);
            else
                animator.SetTrigger(abilityConfig.animParameter.ToString());
        }
    }
}