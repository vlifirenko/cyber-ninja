using System;
using CyberNinja.Config;
using CyberNinja.Ecs.Components.Item;
using CyberNinja.Events;
using CyberNinja.Services.Unit;
using CyberNinja.Views;
using Leopotam.EcsLite;
using UniRx;
using UnityEngine;

namespace CyberNinja.Services.Impl
{
    public class ItemService : IItemService, IDestroyable
    {
        private readonly EcsWorld _world;
        private readonly IUnitService _unitService;
        private readonly EcsPool<ItemComponent> _itemPool;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public ItemService(EcsWorld world, IUnitService unitService)
        {
            _world = world;
            _unitService = unitService;
            _itemPool = _world.GetPool<ItemComponent>();
        }

        public int CreateItem(ItemView view)
        {
            var entity = _world.NewEntity();

            _itemPool.Add(entity);

            return entity;
        }

        public void ActivateItem(int entity)
        {
        }

        public void PickupItem(ItemView view)
        {
            Debug.Log($"Pickup {view.name}");

            var config = view.Config;

            if (config.useOnPickup)
                UseItem(_unitService.GetPlayerEntity(), config);
            else if (config.tryEquip)
                TryEquip(config);

            if (config.destroyAfterPickup)
                view.Hide();

            if (config.reloading > 0)
            {
                view.Model.enabled = false;
                view.Collider.enabled = false;
                Observable.Timer(TimeSpan.FromSeconds(config.reloading))
                    .Subscribe(_ =>
                    {
                        view.Model.enabled = true;
                        view.Collider.enabled = true;
                    })
                    .AddTo(_disposable);
            }
        }

        public void TryEquip(ItemConfig config)
        {
        }

        public void UseItem(int entity, ItemConfig config) => ItemEventsHolder.UseItem(entity, config.useEffect);

        public void OnDestroy() => _disposable.Dispose();
    }
}