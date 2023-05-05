using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Army;
using CyberNinja.Models.Config;
using CyberNinja.Models.Enums;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class SpawnLootSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<SpawnLootComponent>> _filter;
        private EcsPoolInject<EnemyComponent> _enemyPool;
        private EcsCustomInject<GamePrefabsConfig> _gamePrefabsConfig;
        private EcsWorldInject _world;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var enemy = _enemyPool.Value.Get(entity);
                var unit = _world.Value.GetPool<UnitComponent>().Get(entity);
                var loot = enemy.Room.RoomConfig.GetEnemyByType(enemy.Type).loot;
                var randomLootResource = GetRandomLootResource(loot);
                var amount = Random.Range(loot.minResources, loot.maxResources);

                for (var i = 0; i < amount; i++)
                {
                    var resValue = Random.Range(randomLootResource.min, randomLootResource.max);
                    SpawnLootView(randomLootResource.type, resValue, unit.View.Transform.position);    
                }
            }
        }

        private static LootResource GetRandomLootResource(Loot loot)
        {
            var sum = 0f;
            var random = Random.value;
            LootResource randomLoot = null;

            foreach (var item in loot.resources)
            {
                if (random < sum + item.chance)
                {
                    randomLoot = item;
                    break;
                }

                sum += item.chance;
            }

            return randomLoot;
        }

        private void SpawnLootView(EResourceType type, int amount, Vector3 position)
        {
            var spawnPosition = position + new Vector3(Random.Range(-2f, 2f), .8f, Random.Range(-2f, 2f));
            var instance = Object.Instantiate(_gamePrefabsConfig.Value.loot, spawnPosition, Quaternion.identity);

            instance.Type = type;
            instance.Amount = amount;
        }
    }
}