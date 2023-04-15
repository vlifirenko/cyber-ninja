using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Room
{
    public class OnKillEnemySystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsPoolInject<UnitComponent> _unitPool;
        private EcsPoolInject<EnemyComponent> _enemyPool;

        public void Init(IEcsSystems systems) => EnemyEventsHolder.OnKillEnemy += OnKillEnemy;

        private void OnKillEnemy(int entity)
        {
            ref var enemy = ref _enemyPool.Value.Get(entity);

            if (!enemy.Room.EnemyKillMap.ContainsKey(enemy.Type))
                enemy.Room.EnemyKillMap.Add(enemy.Type, 1);
            else
                enemy.Room.EnemyKillMap[enemy.Type] = enemy.Room.EnemyKillMap[enemy.Type] + 1;
            
            Debug.Log(enemy.Room.EnemyKillMap[enemy.Type]);
        }

        public void Destroy(IEcsSystems systems) => EnemyEventsHolder.OnKillEnemy -= OnKillEnemy;
    }
}