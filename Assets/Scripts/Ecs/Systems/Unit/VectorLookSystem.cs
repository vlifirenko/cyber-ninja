using CyberNinja.Ecs.Components.Ai;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Models.Enums;
using CyberNinja.Services.Impl;
using CyberNinja.Services.Unit;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class VectorLookSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitComponent>, Exc<DeadComponent, KnockoutComponent>> _filter;
        private readonly EcsCustomInject<SceneView> _sceneView;
        private readonly EcsCustomInject<GlobalUnitConfig> _characterConfig;
        private readonly EcsCustomInject<UnitService> _unitService;
        private readonly EcsCustomInject<GameData> _gameData;
        private readonly EcsPoolInject<PlayerComponent> _playerPool;
        private readonly EcsPoolInject<MoveVectorComponent> _moveVectorPool;
        private readonly EcsPoolInject<AiTargetComponent> _aiTargetPool;
        private readonly EcsPoolInject<VectorLookComponent> _vectorLookPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var isPlayer = _unitService.Value.GetUnit(entity).Config.ControlType == EControlType.Player;
                UpdateVectorLook(entity, isPlayer);
            }
        }

        private void UpdateVectorLook(int entity, bool isPlayer)
        {
            var view = _unitService.Value.GetUnit(entity).View;
            var finalVectorLook = Vector3.zero;
            var moveVector = _moveVectorPool.Value.Get(entity).Value;

            if (isPlayer)
            {
                var player = _playerPool.Value.Get(entity);

                if (_gameData.Value.inputType == EInputType.Gamepad)
                {
                    var lookVector3 = new Vector3(
                        player.Controls._Player.Look.ReadValue<Vector2>().x,
                        0,
                        player.Controls._Player.Look.ReadValue<Vector2>().y);

                    if (lookVector3 == Vector3.zero)
                    {
                        finalVectorLook = moveVector;
                    }
                    else
                    {
                        finalVectorLook = lookVector3;
                        finalVectorLook = Quaternion.Euler(0, -45, 0) * finalVectorLook.normalized;
                    }
                }
                else
                {
                    var lookVector2 = player.Controls._Player.Look.ReadValue<Vector2>();
                    if (lookVector2 == Vector2.zero)
                        lookVector2 = new Vector2(Screen.width / 2f, Screen.height / 2f); // get center of screen

                    var castPoint = Camera.main.ScreenPointToRay(lookVector2);
                    if (Physics.Raycast(castPoint, out var hit, Mathf.Infinity, _characterConfig.Value.mouseStickLookLayer))
                    {
                        finalVectorLook = hit.point - view.Transform.position;
                        finalVectorLook.y = 0;

                        if (finalVectorLook.magnitude > view.Config.MinMouseMagnitudeTemp)
                        {
                            view.Config.MinMouseMagnitudeTemp = view.Config.MinMouseMagnitude + -view.Config.MouseMagnitudeOffset;
                            finalVectorLook = finalVectorLook.normalized;
                        }
                        else
                        {
                            view.Config.MinMouseMagnitudeTemp = view.Config.MinMouseMagnitude + view.Config.MouseMagnitudeOffset;
                            finalVectorLook = moveVector;
                        }
                    }
                }
            }
            else
            {
                if (_aiTargetPool.Value.Has(entity))
                {
                    finalVectorLook = (view.NavMeshAgent.steeringTarget - view.Transform.position).normalized;
                    finalVectorLook.y = 0;
                }
                else
                {
                    finalVectorLook = moveVector;
                }
            }


            if (finalVectorLook != Vector3.zero)
            {
                if (_vectorLookPool.Value.Has(entity))
                    _vectorLookPool.Value.Get(entity).Value = finalVectorLook;
                else
                    _vectorLookPool.Value.Add(entity).Value = finalVectorLook;
            }
            else
            {
                if (_vectorLookPool.Value.Has(entity))
                    _vectorLookPool.Value.Del(entity);
            }
        }
    }
}