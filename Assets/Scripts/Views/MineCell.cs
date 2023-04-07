using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class MineCell : AView
    {
        [SerializeField] private EMineCircle mineCircle;

        public EMineCircle MineCircle => mineCircle;
    }

    public enum EMineCircle
    {
        None = 0,
        Core = 10,
        Inner = 20,
        Outer = 30
    }
}