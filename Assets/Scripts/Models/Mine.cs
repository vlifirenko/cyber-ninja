using System;

namespace CyberNinja.Models
{
    [Serializable]
    public class Mine
    {
        public bool isOuterMineOpened;
        public MineCircle innerCircle;
        public MineCircle outerCircle;
    }
}