using CyberNinja.Config;
using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class ItemView : AEntityView
    {
        [SerializeField] private ItemConfig config;

        public ItemConfig Config => config;
    }
}