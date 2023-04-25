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
            var isFirstUnit = true;
            foreach (var config in _lobbyConfig.Value.startUnits)
            {
                var armyUnit = new ArmyUnit
                {
                    config = config,
                    level = 1,
                    expMax = 100
                };
                _lobbyData.Value.Army.Add(armyUnit);

                if (isFirstUnit)
                {
                    _lobbyData.Value.selectedArmyUnit = armyUnit;
                    isFirstUnit = false;
                }
            }
        }
    }
}