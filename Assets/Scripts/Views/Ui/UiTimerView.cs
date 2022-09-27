using CyberNinja.Views.Core;
using TMPro;
using UnityEngine;

namespace CyberNinja.Views.Ui
{
    public class UiTimerView : AUiView
    {
        [SerializeField] private TMP_Text timerText;

        private int _seconds;
        private int _minutes;
        private int _hours;
        private int _secondsFromStart;

        private void Update()
        {
            _secondsFromStart = Mathf.RoundToInt(Time.timeSinceLevelLoad);

            _minutes = Mathf.FloorToInt(_secondsFromStart / 60f);
            _hours = Mathf.FloorToInt(_minutes / 60f);
            _seconds = _secondsFromStart - _minutes * 60;

            var s = _seconds < 10 ? "0" + _seconds : _seconds.ToString();
            var m = _minutes < 10 ? "0" + _minutes : _minutes.ToString();
            var h = _hours < 10 ? "0" + _hours : _hours.ToString();

            timerText.text = $"{h}:{m}:{s}";
        }
    }
}