using UnityEngine;

namespace CyberNinja.Views.Core
{
    public abstract class AViewContainer<T> : AView where T : AView
    {
        [SerializeField] protected T[] items;

        public T[] Items => items;
    }
}