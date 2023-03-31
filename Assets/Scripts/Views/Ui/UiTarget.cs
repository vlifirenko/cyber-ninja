using CyberNinja.Views.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Views.Ui
{
    public class UiTarget : AUiView
    {
        [SerializeField] private Image cross;
        [SerializeField] private TMP_Text distance;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TMP_Text lockText;
        [SerializeField] private TMP_Text lockedText;
        [SerializeField] private Slider lockSlider;

        public Image Cross => cross;
        public TMP_Text Distance => distance;
        public Slider HealthSlider => healthSlider;

        public TMP_Text LockText => lockText;

        public Slider LockSlider => lockSlider;

        public TMP_Text LockedText => lockedText;
    }
}