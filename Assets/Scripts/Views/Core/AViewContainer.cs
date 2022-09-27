using UnityEngine;

namespace CyberNinja.Views.Core
{
    public abstract class AViewContainer<T> : AView where T : AView
    {
        [SerializeField] private T[] items;

        public T[] Items => items;
    }
}