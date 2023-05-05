using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Models.Data;
using CyberNinja.Services;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Game
{
    public class InitInputSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private readonly EcsFilterInject<Inc<PlayerComponent>> _filter;
        private readonly EcsPoolInject<PlayerComponent> _playerPool;
        private readonly EcsCustomInject<InputConfig> _inputConfig;
        private readonly EcsCustomInject<SceneView> _sceneView;
        private readonly EcsCustomInject<CanvasView> _canvasView;
        private readonly EcsCustomInject<GameData> _gameData;
        private readonly EcsCustomInject<IGameService> _gameService;

        private Controls _controls;

        public void Init(IEcsSystems systems)
        {
            /*foreach (var entity in _filter.Value)
            {
                ref var player = ref _playerPool.Value.Get(entity);*/

                _controls = new Controls();
                _controls.Enable();
                _gameData.Value.Input = _controls;

                _controls._Game.Quitgame.performed += ctx => _gameService.Value.QuitGame();
                _controls._Game.RestartLevel.performed += ctx => _gameService.Value.ReloadLevel();
                _controls._Game.TogglePostProcessing.performed += ctx => _gameService.Value.TogglePostProcessing();
                _controls._Game.MusicMute.performed += ctx => _gameService.Value.ToggleMusic();
                _controls._Game.ToggleUI.performed += ctx => _gameService.Value.ToggleUI(); //
                _controls._Game.Screenshot.performed += ctx => _gameService.Value.Screenshot();

                _gameData.Value.inputType = _inputConfig.Value.defaultInputType;
            //}
        }

        public void Destroy(IEcsSystems systems) => _controls.Disable();
    }
}