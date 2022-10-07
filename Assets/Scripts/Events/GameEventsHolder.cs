using System;

namespace CyberNinja.Events
{
    public static class GameEventsHolder
    {
        public static event Action<float> OnTimeUpdate;

        public static void UpdateTime(float value) => OnTimeUpdate?.Invoke(value);
    }
}