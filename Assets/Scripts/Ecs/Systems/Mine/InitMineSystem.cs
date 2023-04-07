using System;
using CyberNinja.Models;
using CyberNinja.Models.Config;
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

        [EcsUguiNamed(UiConst.BuyOuterCircle)] private Button _buyOuterCircleButton;
        [EcsUguiNamed(UiConst.ColonyLevelText)] private TMP_Text _colonyLevelText;
        [EcsUguiNamed(UiConst.MessageText)] private TMP_Text _messageText;

        public void Init(IEcsSystems systems)
        {
            foreach (var startResource in _mineConfig.Value.startResources)
                _gameData.Value.playerResources.Map.Add(startResource.type, startResource.value);

            _gameData.Value.colonyLevel = _mineConfig.Value.startColonyLevel;
            _colonyLevelText.text = $"Colony level: {_gameData.Value.colonyLevel}";

            if (_mineConfig.Value.isOuterCircleUnlocked)
                UnlockOuterCircle();
            else
                _buyOuterCircleButton.onClick.AddListener(OnBuyOuterCircle);
        }

        private void OnBuyOuterCircle()
        {
            // todo temp data
            if (_gameData.Value.playerResources.Map[EResourceType.Resource1] < _mineConfig.Value.outerCircleUnlockCost)
            {
                _messageText.text = $"Not enough {EResourceType.Resource1}";
                Observable.Timer(TimeSpan.FromSeconds(2))
                    .Subscribe(_ => _messageText.text = "");
                
                return;
            }

            _gameData.Value.playerResources.Map[EResourceType.Resource1] -= _mineConfig.Value.outerCircleUnlockCost;
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
        }
    }
}