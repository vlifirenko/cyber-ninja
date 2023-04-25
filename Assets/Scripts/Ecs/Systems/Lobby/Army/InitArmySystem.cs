using CyberNinja.Models;
using CyberNinja.Models.Config;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Lobby.Army
{
    public class InitArmySystem : IEcsInitSystem
    {
        private EcsCustomInject<LobbyData> _lobbyData;
        private EcsCustomInject<LobbyConfig> _lobbyConfig;

        public void Init(IEcsSystems systems)
        {
            foreach (var startUnit in _lobbyConfig.Value.startUnits)
            {
                var armyUnit = new ArmyUnit
                {
                    config = startUnit.Config,
                    level = 1
                };
                _lobbyData.Value.army.Add(armyUnit);
            }
        }
    }
}