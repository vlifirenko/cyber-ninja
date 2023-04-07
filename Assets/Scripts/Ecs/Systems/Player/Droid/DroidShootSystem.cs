using System.Collections.Generic;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Services.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CyberNinja.Ecs.Systems.Player.Droid
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

            foreach (var shoot in _shoots.ToArray())
                UpdateProjectile(shoot);
        }

        private void UpdateProjectile(DroidShoot shoot)
        {
            var position = Vector3.Lerp(
                shoot.projectile.Transform.position,
                shoot.target.unitView.Transform.position + new Vector3(0f, 1f, 0f),
                _timer * _globalConfig.Value.droidShootSpeed
            );

            shoot.projectile.Transform.position = position;
            shoot.projectile.Transform.LookAt(shoot.target.unitView.Transform);

            var distance = Vector3.Distance(shoot.projectile.Transform.position, shoot.target.unitView.Transform.position);
            if (distance <= _globalConfig.Value.droidHitDistance)
                OnHit(shoot);
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

        private void OnHit(DroidShoot shoot)
        {
            if (!shoot.target.unitView.Entity.Unpack(_world.Value, out var targetEntity))
                return;

            var damage = _globalConfig.Value.droidDamage;
            
            _unitService.Value.AddDamage(targetEntity, damage, shoot.projectile.Transform);
            shoot.projectile.gameObject.SetActive(false);
            _shoots.Remove(shoot);
        }
    }
}