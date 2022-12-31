using UnityEngine;

namespace CyberNinja.Views.Core
{
    public abstract class AView : MonoBehaviour
    {
        private Transform _transform;

        public Transform Transform
        {
            get
            {
                if (!_transform)
                    _transform = transform;
                return _transform;
            }
        }

        protected virtual void Awake()
        {
            _transform = transform;
        }

        public virtual void Show() => gameObject.SetActive(true);
        
        public virtual void Hide() => gameObject.SetActive(false);
    }
}