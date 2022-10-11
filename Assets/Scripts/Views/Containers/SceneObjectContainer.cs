using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views.Containers
{
    public class SceneObjectContainer : AViewContainer<SceneObjectView>
    {
#if UNITY_EDITOR
        [ContextMenu("Find Items")]
        public void FindItems()
        {
            items = GetComponentsInChildren<SceneObjectView>();
            Debug.Log($"Successfully found {items.Length} items!");
        }
#endif
    }
}