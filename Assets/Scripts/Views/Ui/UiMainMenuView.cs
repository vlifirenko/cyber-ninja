using CyberNinja.Views.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberNinja.Views.Ui
{
    public class UiMainMenuView : AUiView
    {
        public void Continue() => SceneManager.LoadScene(1);

        public void PlayLevel_01() => SceneManager.LoadScene(1);

        public void PlayLevel_02() => SceneManager.LoadScene(2);

        public void PlayLevel_03() => SceneManager.LoadScene(3);

        public void PlayLevel_04() => SceneManager.LoadScene(4);

        public void PlayLevel_05() => SceneManager.LoadScene(5);

        public void QuitGame()
        {
            Debug.Log("QUIT!");
            Application.Quit();
        }
    }
}