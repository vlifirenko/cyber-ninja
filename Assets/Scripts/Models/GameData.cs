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
        public Controls Input;
        public bool isMasterMute;
        public bool isMusicMute;
        public bool isEffectsMute;
        public bool isEnvironmentMute;
        public PlayerResources playerResources = new PlayerResources();
        public int colonyLevel;
        public Mine mine = new Mine();
        public List<RoomView> rooms = new List<RoomView>();
    }

    public enum EGameState
    {
        None = 0,
        Battle = 10,
        Explore = 20
    }
}