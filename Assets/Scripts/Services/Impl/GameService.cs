using CyberNinja.Models;
using CyberNinja.Models.Data;
using CyberNinja.Views;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberNinja.Services.Impl
{
    public class GameService : IGameService
    {
        private readonly EcsWorld _ecsWorld;
        private readonly SceneView _sceneView;
        private readonly CanvasView _canvasView;
        private readonly GameData _gameData;

        public GameService(EcsWorld ecsWorld, SceneView sceneView, CanvasView canvasView, GameData gameData)
        {
            _ecsWorld = ecsWorld;
            _sceneView = sceneView;
            _canvasView = canvasView;
            _gameData = gameData;
        }

        public void QuitGame()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Application.Quit();
            }
            else
            {
                _ecsWorld.Destroy();
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
        }

        public void ReloadLevel()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
                return;

            _ecsWorld.Destroy();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void TogglePostProcessing() => _sceneView.Volume.enabled = !_sceneView.Volume.enabled;

        public void ToggleMusic() => _gameData.isMusicMute = !_gameData.isMusicMute;

        public void ToggleUI() => _canvasView.GameLayout.SetActive(!_canvasView.GameLayout.activeSelf);

        public void Screenshot()
        {
            var folderPath = System.IO.Directory.GetCurrentDirectory() + "/Screenshots/";
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            var screenshotName = $"Screenshot_{System.DateTime.Now:dd-MM-yyyy-HH-mm-ss}.png";
            
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 2);
            Debug.Log(folderPath + screenshotName);
        }
    }
}