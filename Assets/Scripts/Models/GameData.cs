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
        public PlayerResources playerResources = new PlayerResources();
        public int colonyLevel;
        public Mine mine = new Mine();
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
        //public Dictionary<int, MineRoom> rooms = new();
        
        public List<MineRoom> rooms = new List<MineRoom>();

        public void Add(int index, EMineCellState cellState)
        {
            rooms.Add(new MineRoom
            {
                index = index,
                level = cellState
            });
        }

        public void Update(int index, EMineCellState cellState)
        {
            foreach (var item in rooms)
            {
                if (item.index == index)
                {
                    item.level = cellState;
                    break;
                }
            }
        }

        public EMineCellState Get(int index)
        {
            foreach (var item in rooms)
            {
                if (item.index == index)
                    return item.level;
            }

            return EMineCellState.None;
        }
    }

    [Serializable]
    public class MineRoom
    {
        public int index;
        public EMineCellState level;
    }
}