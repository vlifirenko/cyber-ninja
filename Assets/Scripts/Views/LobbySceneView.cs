using CyberNinja.Models.Config;
using UnityEngine;

namespace CyberNinja.Views
{
    public class LobbySceneView : MonoBehaviour
    {
        [SerializeField] private MineCell[] cells;
        [SerializeField] private UnitConfig firstArmyUnitConfig;
        [SerializeField] private Camera upgradeCamera;
        [SerializeField] private CameraView cameraView;
        [SerializeField] private LobbyMinesView minesView;
        [SerializeField] private WormHole wormHole;

        public MineCell[] Cells => cells;

        public UnitConfig FirstArmyUnitConfig => firstArmyUnitConfig;

        public Camera UpgradeCamera => upgradeCamera;

        public CameraView CameraView => cameraView;

        public LobbyMinesView MinesView => minesView;

        public WormHole WormHole => wormHole;
    }
}