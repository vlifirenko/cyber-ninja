using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/SceneObject", fileName = "SceneObject")]
    public class SceneObjectConfig : ScriptableObject
    {
        public bool destroyAfterPickup = true;
        public bool useOnPickup;
        public SceneObjectUseEffect useEffect;
        public float reloading;
        public ItemConfig item;
    }
}