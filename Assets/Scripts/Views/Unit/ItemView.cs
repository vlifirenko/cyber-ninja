using CyberNinja.Models.Config;
using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views.Unit
{
    public class ItemView : AView
    {
        [SerializeField] private ItemConfig config;

        public ItemConfig Config => config;
    }
}