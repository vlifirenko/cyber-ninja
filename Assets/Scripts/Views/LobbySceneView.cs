using CyberNinja.Models.Config;
using UnityEngine;

namespace CyberNinja.Views
{
    public class LobbySceneView : MonoBehaviour
    {
        [SerializeField] private MineCell[] cells;
        [SerializeField] private Camera upgradeCamera;
        [SerializeField] private CameraView cameraView;
        [SerializeField] private LobbyMinesView minesView;
        [SerializeField] private WormHole wormHole;
        [SerializeField] private Hangar hangar;

        public MineCell[] Cells => cells;
                
        public Camera UpgradeCamera => upgradeCamera;

        public CameraView CameraView => cameraView;

        public LobbyMinesView MinesView => minesView;

        public WormHole WormHole => wormHole;

        public Hangar Hangar => hangar;
    }
}