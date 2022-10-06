using CyberNinja.Views.Core;
using CyberNinja.Views.Unit;
using UnityEngine;

namespace CyberNinja.Views.Containers
{
    public class UnitContainer : AViewContainer<UnitView>
    {
        
#if UNITY_EDITOR
        [ContextMenu("Find Items")]
        public void FindItems()
        {
            items = GetComponentsInChildren<UnitView>();
            Debug.Log($"Successfully found {items.Length} items!");
        }
#endif
        
    }
}