using CyberNinja.Models.Config;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Mine
{
    public class InitMineSystem : IEcsInitSystem
    {
        private EcsCustomInject<MineSceneView> _sceneView;
        private EcsCustomInject<MineConfig> _mineConfig;
        
        public void Init(IEcsSystems systems)
        {
            var cells = _sceneView.Value.Cells;

            foreach (var cell in cells)
            {
                if (cell.MineCircle == EMineCircle.Outer)
                    cell.gameObject.SetActive(_mineConfig.Value.isOuterCircleUnlocked);
            }
        }
    }
}