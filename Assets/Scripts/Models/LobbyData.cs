using System;
using System.Collections.Generic;
using CyberNinja.Views;

namespace CyberNinja.Models
{
    [Serializable]
    public class LobbyData
    {
        private List<ArmyUnit> _army = new List<ArmyUnit>();
        
        public ArmyUnit selectedArmyUnit;
        public bool isUpgradeOpened;
        public List<LobbyEnemy> lobbyEnemies = new List<LobbyEnemy>();

        public List<ArmyUnit> Army => _army;
    }

    [Serializable]
    public class LobbyEnemy
    {
        public LobbyMine view;
        public string username;
        public int level;
    }
}