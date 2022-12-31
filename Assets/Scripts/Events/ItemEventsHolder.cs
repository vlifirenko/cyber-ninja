using System;
using CyberNinja.Views;
using CyberNinja.Views.Unit;

namespace CyberNinja.Events
{
    public static class ItemEventsHolder
    {
        public static event Action<SceneObjectView> OnTryPickup;
        public static event Action<ItemView> OnItemTriggerEnter;
        public static event Action<ItemView> OnItemTriggerExit;

        public static void TryPickup(SceneObjectView view) => OnTryPickup?.Invoke(view);
        public static void ItemTriggerEnter(ItemView view) => OnItemTriggerEnter?.Invoke(view);
        public static void ItemTriggerExit(ItemView view) => OnItemTriggerExit?.Invoke(view);
    }
}