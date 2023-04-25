using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class Hangar : AView
    {
        [SerializeField] private GameObject inner;

        public GameObject Inner => inner;
    }
}