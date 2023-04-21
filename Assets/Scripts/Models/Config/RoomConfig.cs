using System;
using CyberNinja.Models.Enums;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(fileName = "Room", menuName = "Config/Room")]
    public class RoomConfig : ScriptableObject
    {
        public RoomEnemyItem[] enemies;
    }
    
    [Serializable]
    public class RoomEnemyItem
    {
        public EEnemyType type;
        public int amount;
    }
}