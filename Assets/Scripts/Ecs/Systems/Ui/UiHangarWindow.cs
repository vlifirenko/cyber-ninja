using CyberNinja.Views.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Ecs.Systems.Ui
{
    public class UiHangarWindow : AUiView
    {
        [SerializeField] private GameObject innerFull;
        [SerializeField] private GameObject innerSmall;
        [SerializeField] private TMP_Text upgradeHealthText;
        [SerializeField] private Button upgradeHealthButton;
        [SerializeField] private Button openFullButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button[] armyButtons;
        [SerializeField] private UiSkillItem attackSkill;
        [SerializeField] private UiSkillItem blockSkill;
        [SerializeField] private TMP_Text unitLevelText;
        [SerializeField] private TMP_Text unitExpText;
        [SerializeField] private Slider unitExpSlider;
        [SerializeField] private TMP_Text unitNameText;

        public TMP_Text UpgradeHealthText => upgradeHealthText;

        public Button UpgradeHealthButton => upgradeHealthButton;

        public GameObject InnerFull => innerFull;

        public GameObject InnerSmall => innerSmall;

        public Button OpenFullButton => openFullButton;

        public Button CloseButton => closeButton;

        public Button[] ArmyButtons => armyButtons;

        public UiSkillItem AttackSkill => attackSkill;

        public UiSkillItem BlockSkill => blockSkill;

        public TMP_Text UnitLevelText => unitLevelText;

        public TMP_Text UnitExpText => unitExpText;

        public Slider UnitExpSlider => unitExpSlider;

        public TMP_Text UnitNameText => unitNameText;
    }
}