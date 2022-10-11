using CyberNinja.Events;
using CyberNinja.Views;
using Leopotam.EcsLite;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Item
{
    public class TryPickupItemService : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            ItemEventsHolder.OnTryPickup += OnTryPickup;
        }

        private void OnTryPickup(SceneObjectView view)
        {
            var config = view.Config;
            var isSuccessPickup = true;
            Debug.Log($"try pickup {config.item.title}");

            if (isSuccessPickup)
            {
                view.Hide();
            }
        }
    }
}