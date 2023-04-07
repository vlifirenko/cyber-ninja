using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Utils;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine.UI;

namespace CyberNinja.Ecs.Systems.Mine
{
    public class InitMineSystem : IEcsInitSystem
    {
        private EcsCustomInject<MineSceneView> _sceneView;
        private EcsCustomInject<MineConfig> _mineConfig;
        private EcsCustomInject<GameData> _gameData;

        [EcsUguiNamed(UiConst.BuyOuterCircle)] private Button _buyOuterCircleButton;
        
        public void Init(IEcsSystems systems)
        {
            var cells = _sceneView.Value.Cells;

            foreach (var cell in cells)
            {
                if (cell.MineCircle == EMineCircle.Outer)
                    cell.gameObject.SetActive(_mineConfig.Value.isOuterCircleUnlocked);
            }

            foreach (var startResource in _mineConfig.Value.startResources)
                _gameData.Value.playerResources.Map.Add(startResource.type, startResource.value);

            _buyOuterCircleButton.onClick.AddListener(OnBuyOuterCircle);
        }

        private void OnBuyOuterCircle()
        {
            
        }
    }
}