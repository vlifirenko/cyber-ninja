using System;
using CyberNinja.Views;
using CyberNinja.Views.Unit;

namespace CyberNinja.Events
{
    public static class ItemEventsHolder
    {
        public static event Action<SceneObjectView> OnTryPickup;
        public static event Action<ItemView> OnPlayerItemTriggerEnter;
        public static event Action<ItemView> OnPlayerItemTriggerExit;

        public static void TryPickup(SceneObjectView view) => OnTryPickup?.Invoke(view);
        public static void PlayerItemTriggerEnter(ItemView view) => OnPlayerItemTriggerEnter?.Invoke(view);
        public static void PlayerItemTriggerExit(ItemView view) => OnPlayerItemTriggerExit?.Invoke(view);
    }
}