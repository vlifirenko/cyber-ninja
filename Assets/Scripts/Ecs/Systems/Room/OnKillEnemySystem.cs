using CyberNinja.Ecs.Components.Room;
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
        private EcsWorldInject _world;

        public void Init(IEcsSystems systems) => EnemyEventsHolder.OnKillEnemy += OnKillEnemy;

        private void OnKillEnemy(int entity)
        {
            ref var enemy = ref _enemyPool.Value.Get(entity);

            if (!enemy.Room.EnemyKillMap.ContainsKey(enemy.Type))
                enemy.Room.EnemyKillMap.Add(enemy.Type, 1);
            else
                enemy.Room.EnemyKillMap[enemy.Type] = enemy.Room.EnemyKillMap[enemy.Type] + 1;

            var roomConfig = enemy.Room.RoomConfig;
            var isRoomClear = true;
            foreach (var item in roomConfig.enemies)
            {
                if (!enemy.Room.EnemyKillMap.ContainsKey(item.type))
                {
                    isRoomClear = false;
                    break;
                }
                
                if (enemy.Room.EnemyKillMap[item.type] < item.amount)
                {
                    isRoomClear = false;
                    break;
                }
            }

            if (isRoomClear)
            {
                var entity1 = _world.Value.NewEntity();
                _world.Value.GetPool<UpdateRoomComponent>().Add(entity1) = new UpdateRoomComponent
                {
                    Room = enemy.Room,
                    IsRoomClear = true
                };
            }
        }

        public void Destroy(IEcsSystems systems) => EnemyEventsHolder.OnKillEnemy -= OnKillEnemy;
    }
}