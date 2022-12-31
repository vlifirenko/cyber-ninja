using CyberNinja.Views.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Views.Ui
{
    public class UiItemPopup : AUiCanvasGroupView
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text titleText;

        public Image IconImage => iconImage;
        public TMP_Text TitleText => titleText;
    }
}