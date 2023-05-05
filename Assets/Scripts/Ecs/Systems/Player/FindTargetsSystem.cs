using System.Collections.Generic;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Models.Unit;
using CyberNinja.Views.Core;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Player
{
    public class FindTargetsSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<
            Inc<UnitComponent>,
            Exc<DeadComponent>> _filter;
        
        private readonly EcsPoolInject<UnitComponent> _unitPool;
        private readonly EcsPoolInject<TargetsComponent> _targetsPool;
        private readonly EcsPoolInject<DeadComponent> _deadPool;
        private readonly EcsCustomInject<AudioConfig> _audioConfig;
        private readonly EcsCustomInject<GlobalUnitConfig> _globalUnitConfig;
        private EcsWorldInject _world;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var unit = _unitPool.Value.Get(entity);
                ref var targets = ref _targetsPool.Value.Get(entity);
                
                var position = unit.View.Transform.position;
                var radius = _globalUnitConfig.Value.lookingTargetDistance;

                var colliders = Physics.SphereCastAll(position, radius, unit.View.Transform.forward, 0f);
                var delItems = new List<Target>(targets.Targets);
                
                foreach (var collider in colliders)
                {
                    if (collider.transform == unit.View.Transform)
                        continue;

                    if (collider.transform.TryGetComponent<AEntityView>(out var unitView))
                    {
                        if (unitView.Entity.Unpack(_world.Value, out var  unitEntity))
                        {
                            if (_deadPool.Value.Has(unitEntity))
                                continue;
                        
                            var distance = Vector3.Distance(position, unitView.Transform.position);
                            var result = GetTarget(entity, unitView);

                            if (result == null)
                                AddTarget(entity, distance, unitView);
                            else
                                // update
                                result.distance = distance;
                        
                            foreach (var item in delItems.ToArray())
                            {
                                if (item.unitView == unitView)
                                    delItems.Remove(item);
                            }   
                        }
                    }
                }

                foreach (var item in delItems)
                    RemoveTarget(entity, item);
            }
        }

        private Target GetTarget(int entity, Object unitView)
        {
            var targets = _targetsPool.Value.Get(entity);
            Target result = null;
            foreach (var item in targets.Targets)
            {
                if (item.unitView == unitView)
                    result = item;
            }

            return result;
        }

        private void AddTarget(int entity, float distance, AEntityView unitView)
        {
            ref var targets = ref _targetsPool.Value.Get(entity);
            if (!unitView.Entity.Unpack(_world.Value, out var targetEntity))
                return;
            var target = new Target
            {
                distance = distance,
                unitView = (UnitView)unitView,
            };
            
            targets.Targets.Add(target);
                CreateUiTarget(targetEntity);
            
            //_audioService.Value.PlayOneShot(_audioConfig.Value.targetFound);
        }

        private void RemoveTarget(int entity, Target target)
        {
            if (!target.unitView.Entity.Unpack(_world.Value, out var targetEntity))
                return;

            if (_world.Value.GetPool<EnemyComponent>().Has(targetEntity))
            {
                var enemy = _world.Value.GetPool<EnemyComponent>().Get(targetEntity);
                enemy.HealthSlider.Target.gameObject.SetActive(false);
            }

            target.Remove();
            
            ref var targets = ref _targetsPool.Value.Get(entity);
            targets.Targets.Remove(target);
        }

        private void CreateUiTarget(int targetEntity)
        {
            if (!_world.Value.GetPool<EnemyComponent>().Has(targetEntity))
                return;
            
            var enemy = _world.Value.GetPool<EnemyComponent>().Get(targetEntity);
            enemy.HealthSlider.Target.gameObject.SetActive(true);
        }
    }
}