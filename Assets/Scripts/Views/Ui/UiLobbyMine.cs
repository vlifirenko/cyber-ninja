using CyberNinja.Views.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Views.Ui
{
    public class UiLobbyMine : AUiView
    {
        [SerializeField] private GameObject inner;
        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Button attackButton;
        [SerializeField] private Button viewButton;

        public TMP_Text UsernameText => usernameText;

        public TMP_Text LevelText => levelText;

        public Button AttackButton => attackButton;

        public Button ViewButton => viewButton;

        public GameObject Inner => inner;
    }
}