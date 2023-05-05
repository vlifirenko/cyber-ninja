using System;

namespace CyberNinja.Models.Mine
{
    [Serializable]
    public class Mine
    {
        public bool isOuterMineOpened;
        public MineCircle innerCircle;
        public MineCircle outerCircle;
    }
}