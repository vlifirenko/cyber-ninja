using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class VfxElectricArc : AView
    {
        [SerializeField] private Transform[] pos;

        public Transform[] Pos => pos;
    }
}