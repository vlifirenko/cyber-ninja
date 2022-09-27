using UnityEngine;

namespace CyberNinja.Views.Core
{
    public abstract class AUiView : AView
    {
        private RectTransform _rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if (!_rectTransform)
                    _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}