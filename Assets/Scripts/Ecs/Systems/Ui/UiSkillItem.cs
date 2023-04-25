using CyberNinja.Views.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Ecs.Systems.Ui
{
    public class UiSkillItem : AUiView
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text sliderText;

        public TMP_Text NameText => nameText;

        public TMP_Text LevelText => levelText;

        public Slider Slider => slider;

        public TMP_Text SliderText => sliderText;
    }
}