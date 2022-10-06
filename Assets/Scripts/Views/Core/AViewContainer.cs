using UnityEngine;

namespace CyberNinja.Views.Core
{
    public abstract class AViewContainer<T> : AView where T : AView
    {
        [SerializeField] private T[] items;

        public T[] Items => items;

#if UNITY_EDITOR
        [ContextMenu("Find Items")]
        private void FindItems()
        {
            items = GetComponentsInChildren<T>();
            Debug.Log($"Successfully found {items.Length} items!");
        }
#endif
    }
}