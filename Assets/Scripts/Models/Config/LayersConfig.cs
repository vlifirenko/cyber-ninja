using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Layers", fileName = "Layers")]
    public class LayersConfig : ScriptableObject
    {
        public LayerMask attackLayer;
        public LayerMask playerLayerMask;
        public LayerMask lobbyMine;
    }
}