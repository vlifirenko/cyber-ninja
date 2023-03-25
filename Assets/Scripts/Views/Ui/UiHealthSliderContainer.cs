using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views.Ui
{
    public class UiHealthSliderContainer : AUiView
    {
        [SerializeField] private UiHealthSlider prefab;
        [SerializeField] private Transform container;

        public UiHealthSlider Prefab => prefab;
        public Transform Container => container;
    }
}