using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace CyberNinja.Models
{
    [Serializable]
    public class LobbyData
    {
        public List<ArmyUnit> army = new List<ArmyUnit>();
        public bool isUpgradeOpened;
    }
}