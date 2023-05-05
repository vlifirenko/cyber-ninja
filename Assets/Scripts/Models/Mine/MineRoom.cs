using System;
using CyberNinja.Views;

namespace CyberNinja.Models.Mine
{
    [Serializable]
    public class MineRoom
    {
        public int index;
        public EMineCellState level;
    }
}