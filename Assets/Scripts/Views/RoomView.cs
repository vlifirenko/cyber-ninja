using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class RoomView : AView
    {
        [SerializeField] private Transform playerSpawn; 
        
        [SerializeField] private EMineCellState _level;
        [SerializeField] private EMineCircle _circle;
        [SerializeField] private int _index;

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

        public Transform PlayerSpawn => playerSpawn;
    }
}