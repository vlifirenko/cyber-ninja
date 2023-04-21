using CyberNinja.Models;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Army
{
    public class AddFirstArmyUnitSystem : IEcsInitSystem
    {
        private EcsCustomInject<LobbyData> _lobbyData;
        private EcsCustomInject<LobbySceneView> _sceneView;

        public void Init(IEcsSystems systems)
        {
            var firstUnit = new ArmyUnit
            {
                config = _sceneView.Value.FirstArmyUnitConfig
            };

            _lobbyData.Value.army.Add(firstUnit);
        }
    }
}