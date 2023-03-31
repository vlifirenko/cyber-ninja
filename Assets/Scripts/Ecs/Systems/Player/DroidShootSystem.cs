using System;
using System.Collections.Generic;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Services.Unit;
using CyberNinja.Views.Projectile;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CyberNinja.Ecs.Systems.Player
{
    public class DroidShootSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsCustomInject<GameData> _gameData;
        private EcsCustomInject<GlobalUnitConfig> _globalConfig;
        private EcsCustomInject<UnitService> _unitService;
        private EcsWorldInject _world;

        private float _reloadTime;
        private float _timer;
        private List<DroidShoot> _shoots = new List<DroidShoot>();

        public void Init(IEcsSystems systems)
        {
            _gameData.Value.Controls._Player.Droid_Shoot.performed += OnDroidShoot;
        }

        public void Run(IEcsSystems systems)
        {
            _reloadTime -= Time.deltaTime;
            _timer += Time.deltaTime;

            foreach (var shoot in _shoots)
            {
                var position = Vector3.Lerp(
                    shoot.projectile.Transform.position,
                    shoot.target.unitView.Transform.position + new Vector3(0f, 1f, 0f),
                    _timer * _globalConfig.Value.droidShootSpeed
                );

                shoot.projectile.Transform.position = position;
                shoot.projectile.Transform.LookAt(shoot.target.unitView.Transform);
            }
        }

        private void OnDroidShoot(InputAction.CallbackContext obj)
        {
            if (_reloadTime > 0f)
                return;

            if (!_unitService.Value.Player.Unpack(_world.Value, out int playerEntity))
                return;
            var unit = _world.Value.GetPool<UnitComponent>().Get(playerEntity);

            _reloadTime = _globalConfig.Value.droidShootReloadTime;
            var instance = Object.Instantiate(_globalConfig.Value.droidProjectile);
            var shoot = new DroidShoot
            {
                projectile = instance,
                target = GetRandomTarget()
            };

            instance.Transform.position = unit.View.DroidView.Transform.position;
            _shoots.Add(shoot);
        }

        private Target GetRandomTarget()
        {
            if (!_unitService.Value.Player.Unpack(_world.Value, out int playerEntity))
                return null;

            var targets = _world.Value.GetPool<TargetsComponent>().Get(playerEntity).Targets;
            if (targets.Count == 0)
                return null;

            var randomTarget = targets[Random.Range(0, targets.Count)];
            return randomTarget;
        }
    }

    [Serializable]
    public class DroidShoot
    {
        public DroidProjectile projectile;
        public Target target;
    }
}