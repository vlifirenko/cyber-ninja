using System.Collections;
using System.Collections.Generic;
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
        private EcsCustomInject<LobbySceneView> _lobbySceneView;

        private LobbyMine _hoveredMine;
        private MeshRenderer _wormHoleRenderer;
        private float _wormHoleTime;

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
            if (_wormHoleRenderer != null)
                UpdateWormHoleRenderer();

            if (Camera.main == null)
                return;

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
            _lobbyMine.UsernameText.text = mine.Data.username.Length == 0 ? "PLAYER" : mine.Data.username;
            _lobbyMine.LevelText.text = $"LEVEL {mine.Data.level}";
            _lobbyMine.Inner.SetActive(true);
        }

        private void UnhoverMine(LobbyMine newMine)
        {
            _lobbyMine.Inner.SetActive(false);
            _hoveredMine = newMine;
        }

        private void OnViewButton()
        {
        }

        private void OnAttackButton()
        {
            _lobbyMine.Inner.gameObject.SetActive(false);

            var wormHole = _lobbySceneView.Value.WormHole;
            Camera.main.gameObject.SetActive(false);
            wormHole.Camera.gameObject.SetActive(true);
            _wormHoleRenderer = wormHole.Renderer;

            SaveService.Save(_lobbyData.Value);
            _lobbySceneView.Value.StartCoroutine(LoadAsyncScene());
        }

        private void UpdateWormHoleRenderer()
        {
            var material = _wormHoleRenderer.material;
            material.SetFloat("_Manual_time", Time.time * _lobbyConfig.Value.wormHoleSpeed);

            _wormHoleTime += Time.deltaTime;
        }

        private IEnumerator LoadAsyncScene()
        {
            var asyncLoad = SceneManager.LoadSceneAsync(_lobbyConfig.Value.gameSceneName, LoadSceneMode.Single);
            asyncLoad.allowSceneActivation = false;
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f && _wormHoleTime >= _lobbyConfig.Value.minWormHoleTime)
                {
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }

            //bLoadDone = asyncLoad.isDone;
        }
    }
}