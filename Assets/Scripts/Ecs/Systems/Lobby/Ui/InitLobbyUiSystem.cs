using CyberNinja.Ecs.Systems.Ui;
using CyberNinja.Models;
using CyberNinja.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;

namespace CyberNinja.Ecs.Systems.Lobby.Ui
{
    public class InitLobbyUiSystem : IEcsInitSystem
    {
        private EcsCustomInject<LobbyData> _lobbyData;
        
        [EcsUguiNamed(UiConst.UpgradeWindow)] private UiUpgradeWindow _upgradeWindow;
        
        public void Init(IEcsSystems systems)
        {
            var armyUnit = _lobbyData.Value.army[0];
            
            //_upgradeWindow.
        }
    }
}