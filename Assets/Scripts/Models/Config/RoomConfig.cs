using System;
using System.Linq;
using CyberNinja.Models.Enums;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(fileName = "Room", menuName = "Config/Room")]
    public class RoomConfig : ScriptableObject
    {
        public RoomEnemyItem[] enemies;

        public RoomEnemyItem GetEnemyByType(EEnemyType type) => enemies.FirstOrDefault(item => item.type == type);
        
        public bool debug_NoSpawnEnemies;
    }
    
    [Serializable]
    public class RoomEnemyItem
    {
        public EEnemyType type;
        public int amount;
        public Loot loot;
    }

    [Serializable]
    public class Loot
    {
        public LootResource[] resources;
        public int minResources;
        public int maxResources;
    }

    [Serializable]
    public class LootResource
    {
        public EResourceType type;
        public float chance;
        public int min;
        public int max;
    }
}