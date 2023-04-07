using System;
using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class MineCell : AView
    {
        [SerializeField] private EMineCircle mineCircle;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material hoverMaterial;

        [SerializeField]private bool _isHovered;
        private Renderer _renderer;

        protected override void Awake()
        {
            base.Awake();
            _renderer = GetComponentInChildren<Renderer>();
        }

        public EMineCircle MineCircle => mineCircle;

        public bool IsHovered
        {
            get => _isHovered;
            set => _isHovered = value;
        }

        private void Update()
        {
            if (_isHovered && _renderer.material != hoverMaterial)
                _renderer.material = hoverMaterial;
            else if (!_isHovered && _renderer.material != defaultMaterial)
                _renderer.material = defaultMaterial;
        }
    }

    public enum EMineCircle
    {
        None = 0,
        Core = 10,
        Inner = 20,
        Outer = 30
    }
}