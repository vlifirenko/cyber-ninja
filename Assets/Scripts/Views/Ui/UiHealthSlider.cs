using CyberNinja.Views.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Views.Ui
{
    public class UiHealthSlider : AUiView
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Vector2 offset;

        public Slider Slider => slider;

        public Vector2 Offset => offset;
    }
}