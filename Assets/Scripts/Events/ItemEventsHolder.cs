using System;
using CyberNinja.Models.Config;
using CyberNinja.Views;

namespace CyberNinja.Events
{
    public static class ItemEventsHolder
    {
        public static event Action<SceneObjectView> OnTryPickup;

        public static void TryPickup(SceneObjectView view) => OnTryPickup?.Invoke(view);
    }
}