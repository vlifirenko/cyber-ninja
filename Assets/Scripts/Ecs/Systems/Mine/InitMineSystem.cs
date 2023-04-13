using System;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Services;
using CyberNinja.Utils;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace CyberNinja.Ecs.Systems.Mine
{
    public class InitMineSystem : IEcsInitSystem
    {
        private EcsCustomInject<MineSceneView> _sceneView;
        private EcsCustomInject<MineConfig> _mineConfig;
        private EcsCustomInject<GameData> _gameData;
        private EcsCustomInject<SaveService> _saveService;

        [EcsUguiNamed(UiConst.BuyOuterCircle)] private Button _buyOuterCircleButton;

        [EcsUguiNamed(UiConst.ColonyLevelText)]
        private TMP_Text _colonyLevelText;

        [EcsUguiNamed(UiConst.MessageText)] private TMP_Text _messageText;

        public void Init(IEcsSystems systems)
        {
            var mine = _gameData.Value.mine;

            if (mine.innerCircle.rooms.Count == 0)
            {
                mine.innerCircle = new MineCircle();
                for (var i = 0; i < 9; i++)
                    mine.innerCircle.Add(i, EMineCellState.Level1);
            }

            if (mine.outerCircle.rooms.Count == 0)
            {
                mine.outerCircle = new MineCircle();
                for (var i = 0; i < 16; i++)
                    mine.outerCircle.Add(i, EMineCellState.Level1);
            }

            // resources
            if (_gameData.Value.playerResources.items.Count == 0)
            {
                foreach (var startResource in _mineConfig.Value.startResources)
                    _gameData.Value.playerResources.Add(startResource.type, startResource.value);
            }

            var input = _gameData.Value.Controls;
            // debug
            input.Debug.Enable();
            input.Debug.AddResource1.performed += _
                =>
            {
                _gameData.Value.playerResources.Update(EResourceType.Resource1, 100);
                SaveService.Save(_gameData.Value);
            };

            // ui
            _gameData.Value.colonyLevel = _mineConfig.Value.startColonyLevel;
            _colonyLevelText.text = $"Colony level: {_gameData.Value.colonyLevel}";

            if (_gameData.Value.mine.isOuterMineOpened)
                UnlockOuterCircle();
            else
                _buyOuterCircleButton.onClick.AddListener(OnBuyOuterCircle);

            // prepare mine cells
            var cells = _sceneView.Value.Cells;
            foreach (var cell in cells)
            {
                if (cell.MineCircle == EMineCircle.Inner)
                {
                    var cellState = _gameData.Value.mine.innerCircle.Get(cell.Index);
                    cell.MineCellState = cellState;
                }
                else if (cell.MineCircle == EMineCircle.Outer)
                {
                    var cellState = _gameData.Value.mine.outerCircle.Get(cell.Index);
                    cell.MineCellState = cellState;
                }
            }
        }

        private void OnBuyOuterCircle()
        {
            // todo temp data
            if (_gameData.Value.playerResources.Get(EResourceType.Resource1) < _mineConfig.Value.outerCircleUnlockCost)
            {
                _messageText.text = $"Not enough {EResourceType.Resource1}";
                Observable.Timer(TimeSpan.FromSeconds(2))
                    .Subscribe(_ => _messageText.text = "");

                return;
            }

            _gameData.Value.playerResources.Update(EResourceType.Resource1, -_mineConfig.Value.outerCircleUnlockCost);
            UnlockOuterCircle();
        }

        private void UnlockOuterCircle()
        {
            var cells = _sceneView.Value.Cells;

            foreach (var cell in cells)
            {
                if (cell.MineCircle == EMineCircle.Outer)
                    cell.gameObject.SetActive(true);
            }

            _buyOuterCircleButton.gameObject.SetActive(false);
            if (!_gameData.Value.mine.isOuterMineOpened)
            {
                _gameData.Value.mine.isOuterMineOpened = true;
                SaveService.Save(_gameData.Value);
            }
        }
    }
}