using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views.Containers
{
    public class ItemContainer : AViewContainer<ItemView>
    {
#if UNITY_EDITOR
        [ContextMenu("Find Items")]
        public void FindItems()
        {
            items = GetComponentsInChildren<ItemView>();
            Debug.Log($"Successfully found {items.Length} items!");
        }
#endif
    }
}