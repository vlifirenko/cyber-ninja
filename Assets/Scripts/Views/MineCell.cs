using System;
using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class MineCell : AView
    {
        [SerializeField] private EMineCircle mineCircle;
        [SerializeField] private int index;
        [SerializeField] private EMineCellState mineCellState;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material level2Material;
        [SerializeField] private Material level3Material;
        [SerializeField] private Material hoverMaterial;

        [SerializeField] private bool _isHovered;
        private Renderer _renderer;

        protected override void Awake()
        {
            base.Awake();
            _renderer = GetComponentInChildren<Renderer>();
            mineCellState = EMineCellState.Level1;
        }

        public EMineCircle MineCircle => mineCircle;

        public bool IsHovered
        {
            get => _isHovered;
            set
            {
                if (mineCircle != EMineCircle.Core)
                {
                    if (!_isHovered && value)
                        _renderer.material = hoverMaterial;
                    else if (_isHovered && !value)
                    {
                        switch (mineCellState)
                        {
                            case EMineCellState.Level1:
                                _renderer.material = defaultMaterial;
                                break;
                            case EMineCellState.Level2:
                                _renderer.material = level2Material;
                                break;
                            case EMineCellState.Level3:
                                _renderer.material = level3Material;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }

                _isHovered = value;
            }
        }

        public EMineCellState MineCellState
        {
            get => mineCellState;
            set => mineCellState = value;
        }

        public int Index => index;
    }

    public enum EMineCircle
    {
        None = 0,
        Core = 10,
        Inner = 20,
        Outer = 30
    }

    public enum EMineCellState
    {
        None = 0,
        Level1 = 10,
        Level2 = 20,
        Level3 = 30
    }
}