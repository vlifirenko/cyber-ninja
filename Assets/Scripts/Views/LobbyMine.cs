using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class LobbyMine : AView
    {
        [SerializeField] private bool _isHovered;
        
        public bool IsHovered
        {
            get => _isHovered;
            set => _isHovered = value;
        }
    }
}