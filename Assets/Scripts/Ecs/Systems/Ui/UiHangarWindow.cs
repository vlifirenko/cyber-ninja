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

        public TMP_Text UpgradeHealthText => upgradeHealthText;

        public Button UpgradeHealthButton => upgradeHealthButton;

        public GameObject InnerFull => innerFull;

        public GameObject InnerSmall => innerSmall;

        public Button OpenFullButton => openFullButton;

        public Button CloseButton => closeButton;
    }
}