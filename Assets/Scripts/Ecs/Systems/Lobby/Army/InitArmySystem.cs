using CyberNinja.Models;
using CyberNinja.Models.Army;
using CyberNinja.Models.Config;
using CyberNinja.Models.Data;
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
            foreach (var item in _lobbyConfig.Value.startArmy)
            {
                var armyUnit = new ArmyUnit
                {
                    unitConfig = item.unitConfig,
                    level = 1,
                    expMax = 100,
                    armyConfig = item
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