namespace CyberNinja.Services
{
    public interface IGameService
    {
        public void QuitGame();

        public void ReloadLevel();

        public void TogglePostProcessing();

        public void ToggleMusic();

        public void ToggleUI();

        public void Screenshot();
    }
}