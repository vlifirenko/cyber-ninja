using System.Collections.Generic;
using CyberNinja.Views.Core;
using TMPro;
using UnityEngine;

namespace CyberNinja.Views.Ui
{
    public class InfoBlockView : AUiView
    {
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private GameObject[] keyboardText;
        [SerializeField] private GameObject[] gamepadText;

        public TMP_Text InfoText => infoText;
        public IEnumerable<GameObject> KeyboardText => keyboardText;
        public IEnumerable<GameObject> GamepadText => gamepadText;
    }
}