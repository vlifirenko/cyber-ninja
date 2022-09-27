using UnityEngine;

namespace CyberNinja.Config
{
    [CreateAssetMenu(menuName = "Config/Layers", fileName = "Layers")]
    public class LayersConfig : ScriptableObject
    {
        public LayerMask attackLayer;
        public LayerMask playerLayerMask;
    }
}