using CyberNinja.Views.Core;
using CyberNinja.Views.Ui;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberNinja.Views
{
    public class CanvasView : AView
    {
        [SerializeField] private GameObject gameLayout;
        [TitleGroup("Attributes")]
        [SerializeField] private GameObject playerHealthBar;
        [SerializeField] private TMP_Text playerHealthText;
        [SerializeField] private GameObject playerEnergyBar;
        [SerializeField] private TMP_Text playerEnergyText;

        [TitleGroup("Abilities")]
        [SerializeField] private Image[] abilityImages;
        [SerializeField] private TMP_Text[] abilityCooldownTexts;

        [TitleGroup("UI Health")]
        [SerializeField] private Transform healthContainer;
        [SerializeField] private GameObject healthPrefab;

        [Space] 
        [SerializeField] private InfoBlockView infoBlockView;

        public GameObject GameLayout => gameLayout;
        public GameObject PlayerHealthBar => playerHealthBar;
        public TMP_Text PlayerHealthText => playerHealthText;
        public GameObject PlayerEnergyBar => playerEnergyBar;
        public TMP_Text PlayerEnergyText => playerEnergyText;
        public Image[] AbilityImages => abilityImages;
        public TMP_Text[] AbilityCooldownTexts => abilityCooldownTexts;
        public Transform HealthContainer => healthContainer;
        public GameObject HealthPrefab => healthPrefab;
        public InfoBlockView InfoBlockView => infoBlockView;
        
        public Canvas Canvas { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Canvas = GetComponent<Canvas>();
        }
    }
}