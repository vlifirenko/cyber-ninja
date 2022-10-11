using System;
using CyberNinja.Models;

namespace CyberNinja.Events
{
    public static class SceneEventsHolder
    {
        public static event Action<int, SceneObjectUseEffect> OnUseSceneObject;

        public static void UseSceneObject(int entity, SceneObjectUseEffect effect) => OnUseSceneObject?.Invoke(entity, effect);
    }
}