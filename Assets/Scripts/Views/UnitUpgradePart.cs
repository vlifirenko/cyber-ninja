using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class UnitUpgradePart : AView
    {
        [SerializeField] private EUnitUpgradePart part;

        public EUnitUpgradePart Part => part;
    }

    public enum EUnitUpgradePart
    {
        None = 0,
        Body = 10,
        Hand = 20,
        Legs = 30
    }
}