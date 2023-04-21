using System;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Lobby", fileName = "Lobby")]
    public class LobbyConfig : ScriptableObject
    {
        public StartResource[] startResources;
        public int outerCircleUnlockCost = 100;
        public int startColonyLevel = 1;
        public LayerMask mineCellLayer;
        public float mineUpgrade2Cost = 200f;
        public float mineUpgrade3Cost = 500f;
        public LayerMask unitPartLayer;
        public float zoomSpeed = 1f;
        [Header("Enemy")]
        public Vector2 startEnemyCount = new Vector2(5, 10);
        public Vector2 mineOffset;
    }

    [Serializable]
    public class StartResource
    {
        public EResourceType type;
        public float value;
    }
}