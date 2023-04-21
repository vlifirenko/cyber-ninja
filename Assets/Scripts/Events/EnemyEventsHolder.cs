using System;

namespace CyberNinja.Events
{
    public static class EnemyEventsHolder
    {
        public static event Action<int> OnKillEnemy;

        public static void InvokeOnKillEnemy(int entity) => OnKillEnemy?.Invoke(entity);
    }
}