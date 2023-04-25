using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Army
{
    public class AddFirstArmyUnitSystem : IEcsInitSystem
    {
        private EcsCustomInject<LobbyData> _lobbyData;
        private EcsCustomInject<LobbyConfig> _lobbyConfig;

        public void Init(IEcsSystems systems)
        {
            foreach (var startUnit in _lobbyConfig.Value.startUnits)
            {
                var armyUnit = new ArmyUnit
                {
                    config = startUnit.Config
                };
                _lobbyData.Value.army.Add(armyUnit);
            }
        }
    }
}