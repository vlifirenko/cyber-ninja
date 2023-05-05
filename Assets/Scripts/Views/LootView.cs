using CyberNinja.Models.Army;
using CyberNinja.Models.Enums;
using CyberNinja.Views.Core;

namespace CyberNinja.Views
{
    public class LootView : AView
    {
        public EResourceType Type { get; set; }
        public int Amount { get; set; }
    }
}