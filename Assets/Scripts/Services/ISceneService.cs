using CyberNinja.Views;

namespace CyberNinja.Services
{
    public interface ISceneService
    {
        public void TriggerObject(SceneObjectView view);
        
        public int CreateObject(SceneObjectView view);
    }
}