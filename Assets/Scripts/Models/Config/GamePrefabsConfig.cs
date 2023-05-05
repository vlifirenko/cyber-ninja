using CyberNinja.Views;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(fileName = "GamePrefabConfig", menuName = "Config/Game Prefabs")]
    public class GamePrefabsConfig : ScriptableObject
    {
        public LootView loot;
    }
}