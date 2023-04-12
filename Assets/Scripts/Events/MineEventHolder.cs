using System;

namespace CyberNinja.Events
{
    public static class MineEventHolder
    {
        public static event Action OnMineUpdate;

        public static void InvokeOnMineUpdate() => OnMineUpdate?.Invoke();
    }
}