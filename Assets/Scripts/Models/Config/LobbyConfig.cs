using System;
using CyberNinja.Views.Unit;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Lobby", fileName = "Lobby")]
    public class LobbyConfig : ScriptableObject
    {
        public string gameSceneName;
        public StartResource[] startResources;
        public int outerCircleUnlockCost = 100;
        public int startColonyLevel = 1;
        public LayerMask mineCellLayer;
        public float mineUpgrade2Cost = 200f;
        public float mineUpgrade3Cost = 500f;
        public float zoomSpeed = 1f;
        public UnitConfig[] startUnits;
        public float hangarCameraSpeed = 2f;
        [Header("Enemy")]
        public Vector2 startEnemyCount = new Vector2(5, 10);
        public Vector2 mineOffset;
        public Vector2Int enemyLevelRange;
        public float wormHoleSpeed = 2f;
        public float minWormHoleTime = 3f;
    }

    [Serializable]
    public class StartResource
    {
        public EResourceType type;
        public float value;
    }
}