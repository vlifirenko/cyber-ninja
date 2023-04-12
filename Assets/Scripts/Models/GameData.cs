using System;
using System.Collections.Generic;
using CyberNinja.Models.Enums;
using CyberNinja.Views;

namespace CyberNinja.Models
{
    [Serializable]
    public class GameData
    {
        public EInputType inputType;
        public Controls Controls;
        public bool isMasterMute;
        public bool isMusicMute;
        public bool isEffectsMute;
        public bool isEnvironmentMute;
        public PlayerResources playerResources = new();
        public int colonyLevel;
        public Mine mine = new();
    }

    [Serializable]
    public class Mine
    {
        public bool isOuterMineOpened;
        public MineCircle innerCircle;
        public MineCircle outerCircle;
    }

    [Serializable]
    public class MineCircle
    {
        public Dictionary<int, MineRoom> rooms = new();
    }

    [Serializable]
    public class MineRoom
    {
        public EMineCellState level;
    }
}