using System;

namespace CyberNinja.Event
{
    public static class EventsHolder
    {
        public static event Action<float> OnTimeUpdate;

        public static void UpdateTime(float value) => OnTimeUpdate?.Invoke(value);
    }
}