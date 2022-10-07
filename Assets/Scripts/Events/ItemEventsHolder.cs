using System;
using CyberNinja.Config;
using CyberNinja.Models.Enums;

namespace CyberNinja.Events
{
    public static class ItemEventsHolder
    {
        public static event Action<int, ItemUseEffect> OnUseItem;

        public static void UseItem(int entity, ItemUseEffect effect) => OnUseItem?.Invoke(entity, effect);
    }
}