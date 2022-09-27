using System.Collections;
using CyberNinja.Views.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Views.Ui
{
    public class UiBlackLoadView : AUiView
    {
        private readonly Color _startColor = new Color(0, 0, 0, 100);
        private readonly Color _targetColor = new Color(0, 0, 0, 0);

        [SerializeField] private Image image;
        [SerializeField] private float time = 1;
        [SerializeField] private float delay;


        private void OnEnable()
        {
            image.color = _startColor;
            StartCoroutine(CrossFade(delay));
            StartCoroutine(Deactivate(time + delay + 1));
        }

        private IEnumerator CrossFade(float d)
        {
            yield return new WaitForSeconds(d);
            image.CrossFadeColor(_targetColor, time, true, true);
        }

        private IEnumerator Deactivate(float d)
        {
            yield return new WaitForSeconds(d);
            gameObject.SetActive(false);
        }
    }
}