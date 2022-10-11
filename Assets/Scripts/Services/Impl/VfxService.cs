using System;
using CyberNinja.Ecs.Components;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Ability;
using CyberNinja.Models.Config;
using CyberNinja.Views;
using Leopotam.EcsLite;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CyberNinja.Services.Impl
{
    public class VfxService : IVfxService
    {
        private readonly EcsWorld _world;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public VfxService(EcsWorld world)
        {
            _world = world;
        }

        public void SpawnVfx(int entity, AbilityConfig abilityConfig, bool hit = false,
            float hitDamageClamped = 0, float hitBloodClamped = 0, Vector3? hitOrigin = null)
        {
            var unit = _world.GetPool<UnitComponent>().Get(entity);
            
            var vfx = Object.Instantiate(abilityConfig.vfxGameobject, unit.View.VfxSpawnPoint);
            vfx.transform.localScale = new Vector3(
                1 / vfx.transform.lossyScale.x,
                1 / vfx.transform.lossyScale.y,
                1 / vfx.transform.lossyScale.z);
            vfx.transform.localPosition = abilityConfig.vfxPosition;
            
            var vfxController = vfx.GetComponent<VfxView>();
            vfxController.VfxLifetime = abilityConfig.vfxLifeTime;

            if (abilityConfig.useRadius)
                vfxController.EffectRadius = abilityConfig.radius;
            else
                vfxController.EffectRadius = 111;

            if (hit)
            {
                if (abilityConfig.vfxPlaceOutside)
                    vfx.transform.SetParent(null);
                vfxController.VfxDamage = hitDamageClamped;
                vfxController.VfxBlood = hitBloodClamped;

                if (hitOrigin != null)
                {
                    var hitAngle = Quaternion.FromToRotation(Vector3.forward,
                            hitOrigin.Value - unit.View.Transform.position).eulerAngles;
                    hitAngle = new Vector3(0, hitAngle.y + 180, 0);
                    vfx.transform.localEulerAngles = hitAngle;
                }
                else
                {
                    vfx.transform.localEulerAngles = abilityConfig.vfxRotation;
                }
            }
            else
            {
                vfx.transform.localEulerAngles = abilityConfig.vfxRotation;//
                if (abilityConfig.vfxPlaceOutside) vfx.transform.SetParent(null);//
            }

            
            Observable.Timer(TimeSpan.FromSeconds(abilityConfig.vfxLifeTime))
                .Subscribe(_ => Object.Destroy(vfx))
                .AddTo(_disposable);
        }

        public void OnDestroy() => _disposable.Dispose();
    }
}