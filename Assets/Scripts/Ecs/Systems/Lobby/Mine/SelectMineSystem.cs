using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Services;
using CyberNinja.Utils;
using CyberNinja.Views;
using CyberNinja.Views.Ui;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace CyberNinja.Ecs.Systems.Lobby.Mine
{
    public class SelectMineSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsCustomInject<LobbyConfig> _lobbyConfig;
        private EcsCustomInject<LayersConfig> _layersConfig;
        private EcsCustomInject<LobbyData> _lobbyData;
        private EcsCustomInject<SaveService> _saveService;

        private LobbyMine _hoveredMine;

        [EcsUguiNamed(UiConst.LobbyMine)] private UiLobbyMine _lobbyMine;
        [EcsUguiNamed(UiConst.MessageText)] private TMP_Text _messageText;
        [EcsUguiNamed(UiConst.Canvas)] private Canvas _canvas;

        public void Init(IEcsSystems systems)
        {
            _lobbyMine.AttackButton.onClick.AddListener(OnAttackButton);
            _lobbyMine.ViewButton.onClick.AddListener(OnViewButton);
        }

        public void Run(IEcsSystems systems)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _layersConfig.Value.lobbyMine))
            {
                var mine = hit.transform.parent.GetComponent<LobbyMine>();

                if (mine == _hoveredMine)
                    return;

                if (_hoveredMine == null)
                    HoverMine(mine);
                else if (_hoveredMine != null)
                {
                    UnhoverMine(mine);
                    HoverMine(mine);
                }
            }
            else if (_hoveredMine != null && !EventSystem.current.IsPointerOverGameObject())
                UnhoverMine(null);
        }

        private void HoverMine(LobbyMine mine)
        {
            _hoveredMine = mine;
            
            var uiPosition = _canvas.WorldToCanvasPosition(mine.Transform.position);
            _lobbyMine.GetComponent<RectTransform>().anchoredPosition = uiPosition;
            _lobbyMine.Inner.SetActive(true);
        }

        private void UnhoverMine(LobbyMine newMine)
        {
            _lobbyMine.Inner.SetActive(false);
            _hoveredMine = newMine;
        }
        
        private void OnViewButton()
        {
            throw new System.NotImplementedException();
        }

        private void OnAttackButton()
        {
            SaveService.Save(_lobbyData.Value);
            SceneManager.LoadScene(_lobbyConfig.Value.gameSceneName);
        }
    }
}