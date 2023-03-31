using System;
using CyberNinja.Ecs.Components;
using CyberNinja.Ecs.Components.Door;
using CyberNinja.Models.Enums;
using CyberNinja.Services.Unit;
using CyberNinja.Utils;
using CyberNinja.Views;
using Leopotam.EcsLite;
using UniRx;
using UnityEngine;

namespace CyberNinja.Services.Impl
{
    public class DoorService : IDoorService
    {
        private readonly EcsWorld _world;
        private readonly UnitService _unitService;
        private readonly CompositeDisposable _disposable;

        public DoorService(EcsWorld world, UnitService unitService)
        {
            _world = world;
            _unitService = unitService;
        }

        public void CreateDoor(DoorView view)
        {
            var entity = _world.NewEntity();
            var door = new DoorComponent
            {
                IsInteractable = true,
                View = view,
                TargetDoor = view.TargetDoor
            };

            _world.GetPool<DoorComponent>().Add(entity) = door;
            view.Entity = _world.PackEntity(entity);
        }

        public void TryActivateDoor(DoorView view, int unitEntity)
        {
            if (!view.Entity.Unpack(_world, out var doorEntity))
                throw new Exception("Unpack error");

            var door = _world.GetPool<DoorComponent>().Get(doorEntity);
            if (door.IsInteractable)
                ActivateDoor(doorEntity, unitEntity);
        }

        private void ActivateDoor(int doorEntity, int unitEntity)
        {
            ref var door = ref _world.GetPool<DoorComponent>().Get(doorEntity);
            var delayTime = 0f;
            var cooldownTime = 0f;

            if (door.View.Parameters)
            {
                delayTime = door.View.Delay;
                cooldownTime = door.View.Cooldown;
            }
            else if (door.View.Automatic)
            {
                delayTime = door.View.AutomaticDelay;
                cooldownTime = door.View.AutomaticDelay;
            }

            if (door.View.Vfx)
            {
                Observable.Timer(TimeSpan.FromSeconds(door.View.VfxSpawnDelay))
                    .Subscribe(_ =>
                    {
                        ref var doorView = ref _world.GetPool<DoorComponent>().Get(doorEntity).View;
                        SpawnVfx(doorView.VfxActivationPrefab, doorView.Transform, doorView.VfxLifeTime);
                    })
                    .AddTo(_disposable);
            }
            
            if (delayTime != 0)
            {
                Observable.Timer(TimeSpan.FromSeconds(delayTime))
                    .Subscribe(_ =>
                    {
                        ref var doorView = ref _world.GetPool<DoorComponent>().Get(doorEntity).View;
                        NavmeshWarp(doorView.Transform, unitEntity);
                    })
                    .AddTo(_disposable);
            }
            else
                NavmeshWarp(door.View.Transform, unitEntity);

            
            if (cooldownTime != 0)
            {
                door.IsInteractable = false;
                Observable.Timer(TimeSpan.FromSeconds(delayTime + cooldownTime))
                    .Subscribe(_ =>
                    {
                        ref var doorComponent = ref _world.GetPool<DoorComponent>().Get(doorEntity);
                        doorComponent.IsInteractable = true;
                    })
                    .AddTo(_disposable);
            }
        }

        private void SpawnVfx(GameObject prefab, Transform parent, float lifeTime)
        {
            var vfx = UnityEngine.Object.Instantiate(prefab, parent);
            var vfxController = vfx.GetComponent<VfxView>();

            vfxController.VfxLifetime = lifeTime;
            Observable.Timer(TimeSpan.FromSeconds(lifeTime))
                .Subscribe(_ => UnityEngine.Object.Destroy(vfx))
                .AddTo(_disposable);
        }

        private void NavmeshWarp(Component targetDoor, int unitEntity)
        {
            var unit = _unitService.GetUnit(unitEntity);
            Vector3 targetDoorPosition;

            if (targetDoor.CompareTag(Tag.Door))
            {
                var outPosition = targetDoor.GetComponent<DoorView>().OutPosition.position;
                targetDoorPosition = outPosition;
            }
            else
                targetDoorPosition = targetDoor.transform.position;

            unit.View.NavMeshAgent.Warp(targetDoorPosition);
        }

        public void OnDestroy() => _disposable.Dispose();
    }
}