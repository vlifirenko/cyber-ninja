using UnityEngine;

namespace CyberNinja.Views.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AUiCanvasGroupView : AUiView
    {
        [SerializeField] private CanvasGroup canvasGroup;

#if UNITY_EDITOR
        [ContextMenu("Update Components")]
        private void UpdateComponents()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        
        [ContextMenu("Show")]
        private void ShowInternal() => Show();
        [ContextMenu("Hide")]
        private void HideInternal() => Hide();
#endif
        
        public override void Show()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        
        public override void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }
}