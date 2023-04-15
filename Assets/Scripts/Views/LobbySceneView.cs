using CyberNinja.Models.Config;
using UnityEngine;

namespace CyberNinja.Views
{
    public class LobbySceneView : MonoBehaviour
    {
        [SerializeField] private MineCell[] cells;
        [SerializeField] private UnitConfig firstArmyUnitConfig;
        [SerializeField] private Camera upgradeCamera;

        public MineCell[] Cells => cells;

        public UnitConfig FirstArmyUnitConfig => firstArmyUnitConfig;

        public Camera UpgradeCamera => upgradeCamera;
    }
}