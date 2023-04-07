using UnityEngine;

namespace CyberNinja.Views
{
    public class MineSceneView : MonoBehaviour
    {
        [SerializeField] private MineCell[] cells;

        public MineCell[] Cells => cells;
    }
}