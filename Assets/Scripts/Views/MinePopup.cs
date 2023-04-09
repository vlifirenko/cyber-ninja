using CyberNinja.Views.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Views
{
    public class MinePopup : AUiView
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private GameObject inner;
        [SerializeField] private RectTransform window;
        [SerializeField] private Vector2 offset = new Vector2(0, 100);

        public Button CloseButton => closeButton;

        public Button UpgradeButton => upgradeButton;

        public TMP_Text NameText => nameText;

        public TMP_Text LevelText => levelText;

        public GameObject Inner => inner;

        public RectTransform Window => window;

        public Vector2 Offset => offset;
    }
}