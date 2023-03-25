using CyberNinja.Events;
using CyberNinja.Views.Ui;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;

namespace CyberNinja.Ecs.Systems.Ui
{
    public class ItemPopupSystem : IEcsInitSystem
    {
        [EcsUguiNamed(Utils.UiConst.ItemPopup)] private UiItemPopup _itemPopup;

        public void Init(IEcsSystems systems)
        {
            ItemEventsHolder.OnPlayerItemTriggerEnter += OnPlayerItemTriggerEnter;
            ItemEventsHolder.OnPlayerItemTriggerExit += OnPlayerItemTriggerExit;
        }

        private void OnPlayerItemTriggerEnter(ItemView itemView)
        {
            var config = itemView.Config;

            _itemPopup.IconImage.sprite = config.icon;
            _itemPopup.TitleText.text = config.title;
            _itemPopup.Show();
        }

        private void OnPlayerItemTriggerExit(ItemView itemView) => _itemPopup.Hide();
    }
}