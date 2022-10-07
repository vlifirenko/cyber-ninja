using System;
using UnityEngine;

namespace CyberNinja.Events
{
    public static class UnitEventsHolder
    {
        public static event Action<int, Vector2> OnHealthUpdate;
        public static event Action<int, Vector2> OnEnergyUpdate;

        public static void UpdateHealth(int entity, Vector2 value) => OnHealthUpdate?.Invoke(entity, value);
        public static void UpdateEnergy(int entity, Vector2 value) => OnEnergyUpdate?.Invoke(entity, value);
    }
}