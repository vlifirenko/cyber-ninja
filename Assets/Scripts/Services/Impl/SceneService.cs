using System;
using CyberNinja.Ecs.Components.SceneObject;
using CyberNinja.Events;
using CyberNinja.Models.Enums;
using CyberNinja.Services.Unit;
using CyberNinja.Views;
using Leopotam.EcsLite;
using UniRx;
using UnityEngine;

namespace CyberNinja.Services.Impl
{
    public class SceneService : ISceneService, IDestroyable
    {
        private readonly EcsWorld _world;
        private readonly IUnitService _unitService;
        private readonly EcsPool<SceneObjectComponent> _sceneObjectPool;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public SceneService(EcsWorld world, IUnitService unitService)
        {
            _world = world;
            _unitService = unitService;
            
            _sceneObjectPool = _world.GetPool<SceneObjectComponent>();
        }

        public int CreateObject(SceneObjectView view)
        {
            var entity = _world.NewEntity();

            _sceneObjectPool.Add(entity);

            return entity;
        }

        public void TriggerObject(SceneObjectView view)
        {
            Debug.Log($"Collide {view.name}");

            var config = view.Config;
            var playerEntity = _unitService.GetPlayerEntity();
            
            

            // legacy impl
            /*if (config.useOnPickup)
            {
                if (config.useEffect.type == EItemUseEffectType.None)
                    throw new Exception("Config use effect type is None");
                
                SceneEventsHolder.UseSceneObject(_unitService.GetPlayerEntity(), config.useEffect);
                
                if (config.destroyAfterPickup)
                    view.Hide();
            }
            else if (config.tryEquip)
            {
                if (config.item == null)
                    throw new Exception("Scene object config hasn't item");
                
                ItemEventsHolder.TryPickup(view);
            }

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
            }*/
        }

        public void OnDestroy() => _disposable.Dispose();
    }
}