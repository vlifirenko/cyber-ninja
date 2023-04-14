using CyberNinja.Views.Core;

namespace CyberNinja.Views
{
    public class RoomView : AView
    {
        private EMineCellState _level;
        private EMineCircle _circle;
        private int _index;

        public EMineCellState Level
        {
            get => _level;
            set => _level = value;
        }

        public EMineCircle Circle
        {
            get => _circle;
            set => _circle = value;
        }

        public int Index
        {
            get => _index;
            set => _index = value;
        }
    }
}