using CyberNinja.Models;
using CyberNinja.Models.Data;
using CyberNinja.Models.Enums;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Game
{
    public class GameSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<CanvasView> _canvasView;
        private readonly EcsCustomInject<GameData> _gameData;

        public void Init(IEcsSystems systems)
        {
            var infoText = $"{Application.productName} {Application.version}\n{Application.companyName}";
            _canvasView.Value.InfoBlockView.InfoText.text = infoText;

            if (_gameData.Value.inputType == EInputType.Gamepad)
            {
                foreach (var item in _canvasView.Value.InfoBlockView.KeyboardText)
                    item.SetActive(false);

                foreach (var item in _canvasView.Value.InfoBlockView.GamepadText)
                    item.SetActive(true);
            }
            else
            {
                foreach (var item in _canvasView.Value.InfoBlockView.KeyboardText)
                    item.SetActive(true);

                foreach (var item in _canvasView.Value.InfoBlockView.GamepadText)
                    item.SetActive(false);
            }
        }
    }
}