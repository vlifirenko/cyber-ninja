using CyberNinja.Views.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Ecs.Systems.Ui
{
    public class UiUpgradeWindow : AUiView
    {
        [SerializeField] private TMP_Text upgradeHealthText;
        [SerializeField] private Button upgradeHealthButton;
    }
}