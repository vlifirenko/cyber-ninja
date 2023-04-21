using System;
using System.Collections.Generic;
using CyberNinja.Views;
using Leopotam.EcsLite;

namespace CyberNinja.Models
{
    [Serializable]
    public class LobbyData
    {
        public List<ArmyUnit> army = new List<ArmyUnit>();
        public bool isUpgradeOpened;
        public List<LobbyEnemy> lobbyEnemies = new List<LobbyEnemy>();
    }

    [Serializable]
    public class LobbyEnemy
    {
        public LobbyMine view;
        public string username;
    }
}