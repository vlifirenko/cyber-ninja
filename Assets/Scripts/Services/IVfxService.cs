using CyberNinja.Models.Ability;
using CyberNinja.Models.Config;
using UnityEngine;

namespace CyberNinja.Services
{
    public interface IVfxService : IDestroyable
    {
        public void SpawnVfx(int entity, AbilityConfig abilityConfig, bool hit = false,
            float hitDamageClamped = 0, float hitBloodClamped = 0, Vector3? hitOrigin = null);
    }
}