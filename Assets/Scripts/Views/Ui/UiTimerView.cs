using CyberNinja.Views.Core;
using TMPro;
using UnityEngine;

namespace CyberNinja.Views.Ui
{
    public class UiTimerView : AUiView
    {
        [SerializeField] private TMP_Text timerText;

        public TMP_Text TimerText => timerText;
    }
}