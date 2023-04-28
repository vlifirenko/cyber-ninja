using CyberNinja.Ecs.Components.Ai;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models;
using CyberNinja.Models.Enums;
using CyberNinja.Services;
using CyberNinja.Services.Unit;
using CyberNinja.Views;
using CyberNinja.Views.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AI;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class MovementSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<SceneView> _sceneView;
        private readonly EcsFilterInject<Inc<UnitComponent>, Exc<DeadComponent>> _filter;
        private readonly EcsCustomInject<UnitService> _unitService;
        private readonly EcsCustomInject<GameData> _gameData;
        private readonly EcsPoolInject<PlayerComponent> _playerPool;
        private readonly EcsPoolInject<InputAxisComponent> _inputAxisPool;
        private readonly EcsPoolInject<MoveVectorComponent> _moveVectorPool;
        private readonly EcsPoolInject<SpeedComponent> _speedPool;
        private readonly EcsPoolInject<AiTargetComponent> _aiTargetPool;
        private readonly EcsPoolInject<PressMoveDetectedComponent> _pressMoveDetectedPool;
        private readonly EcsPoolInject<StationaryComponent> _stationaryPool;
        private readonly EcsPoolInject<StunComponent> _stunPool;
        private readonly EcsPoolInject<KnockoutComponent> _unconsciousPool;
        private readonly EcsPoolInject<AiTaskComponent> _aiTaskPool;
        private readonly EcsPoolInject<VectorLookComponent> _vectorLookPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var unit = _unitService.Value.GetUnit(entity);
                var isPlayer = unit.Config.ControlType == EControlType.Player;

                VectorMoveMath(entity, isPlayer);
                InputMath(entity, unit.View, isPlayer);

                if (_unconsciousPool.Value.Has(entity))
                    continue;

                SetTranslation(entity, unit.View.NavMeshAgent, isPlayer);
                SetRotation(entity, unit.View.NavMeshAgent, isPlayer);
            }
        }

        private void VectorMoveMath(int entity, bool isPlayer)
        {
            if (isPlayer)
            {
                var controls = _gameData.Value.Input;
                var inputAxis = new Vector2(
                    controls._Player.Move.ReadValue<Vector2>().x,
                    controls._Player.Move.ReadValue<Vector2>().y);

                inputAxis.Normalize();
                if (_inputAxisPool.Value.Has(entity))
                    _inputAxisPool.Value.Get(entity).Value = inputAxis;
                else
                    _inputAxisPool.Value.Add(entity).Value = inputAxis;
                
                var cameraTransform = _sceneView.Value.CameraView.CameraMain;
                var forward = cameraTransform.forward;
                var right = cameraTransform.right;
                forward.y = right.y = 0f;
                forward.Normalize();
                right.Normalize();

                var targetMoveVectorPlayer = forward.normalized * inputAxis.y + right.normalized * inputAxis.x;
                if (_moveVectorPool.Value.Has(entity))
                    _moveVectorPool.Value.Get(entity).Value = targetMoveVectorPlayer;
                else
                    _moveVectorPool.Value.Add(entity).Value = targetMoveVectorPlayer;
            }
            else
            {
                var unit = _unitService.Value.GetUnit(entity);
                var vector = unit.View.Transform.forward;
                vector.y = 0;
                if (_moveVectorPool.Value.Has(entity))
                    _moveVectorPool.Value.Get(entity).Value = vector;
                else
                    _moveVectorPool.Value.Add(entity).Value = vector;
            }
        }

        private void InputMath(int entity, UnitView view, bool isPlayerControl)
        {
            ref var speed = ref _speedPool.Value.Get(entity);
            var speedLerp = Mathf.Lerp(
                speed.SpeedCurrent,
                speed.SpeedTarget,
                view.Config.SmoothMove);
            if (speedLerp < 0.01f)
                speedLerp = 0;

            speed.SpeedCurrent = speedLerp;

            var enemyIsClose = _aiTargetPool.Value.Has(entity)
                               && _aiTargetPool.Value.Get(entity).Distance <= view.Config.AttackDistance;

            if (isPlayerControl)
            {
                var inputAxis = _inputAxisPool.Value.Get(entity);

                if (inputAxis.Value.x != 0 || inputAxis.Value.y != 0)
                {
                    if (!_pressMoveDetectedPool.Value.Has(entity))
                        _pressMoveDetectedPool.Value.Add(entity);
                }
                else if (_pressMoveDetectedPool.Value.Has(entity))
                    _pressMoveDetectedPool.Value.Del(entity);
            }
            else
            {
                if (_aiTargetPool.Value.Has(entity))
                {
                    if (!_pressMoveDetectedPool.Value.Has(entity))
                        _pressMoveDetectedPool.Value.Add(entity);
                }
                else if (_pressMoveDetectedPool.Value.Has(entity))
                    _pressMoveDetectedPool.Value.Del(entity);
            }

            if (_pressMoveDetectedPool.Value.Has(entity) && !_stationaryPool.Value.Has(entity))
            {
                var speedTarget = 0f;
                if (!enemyIsClose)
                {
                    speedTarget = speed.SpeedMoveMax * view.Config.SpeedFactor;
                }

                speed.SpeedTarget = speedTarget;
            }
            else
            {
                speed.SpeedTarget = 0f;
            }
            
            if (_stunPool.Value.Has(entity))
                speed.SpeedTarget = 0f;
        }

        private void SetTranslation(int entity, NavMeshAgent agent, bool isPlayer)
        {
            if (isPlayer) 
            {
                var moveVector = _moveVectorPool.Value.Get(entity);
                var speed = _speedPool.Value.Get(entity);
                var targetTranslation = moveVector.Value * speed.SpeedCurrent * Time.deltaTime;

                agent.Move(targetTranslation);
            }
            else
            {
                if (_pressMoveDetectedPool.Value.Has(entity) && _aiTargetPool.Value.Has(entity))
                {
                    var aiTarget = _aiTargetPool.Value.Get(entity);
                    var aiTask = _aiTaskPool.Value.Get(entity);

                    agent.SetDestination(aiTarget.Transform.position);
                    agent.isStopped = aiTask.Value == EAiTaskType.Attack;
                }
                else
                    agent.ResetPath();
            }
        }

        private void SetRotation(int entity, NavMeshAgent agent, bool isPlayerControl)
        {
            var unit = _unitService.Value.GetUnit(entity);

            if (isPlayerControl)
            {
                Vector3 forwardVector;
                if (unit.Config.RotateToLookVector && _vectorLookPool.Value.Has(entity))
                    forwardVector = _vectorLookPool.Value.Get(entity).Value;
                else
                    forwardVector = _moveVectorPool.Value.Get(entity).Value;

                if (forwardVector != Vector3.zero)
                {
                    var direction = Quaternion.LookRotation(forwardVector, Vector3.up);
                    var rotation = Quaternion.Slerp(
                        unit.View.Transform.rotation,
                        direction,
                        unit.Config.SmoothRotation);
                    unit.View.Transform.rotation = rotation;
                }
            }
            else
            {
                if (!_aiTargetPool.Value.Has(entity))
                    return;

                var targetVector = agent.steeringTarget - unit.View.Transform.position;
                unit.View.Transform.rotation = Quaternion.Slerp(
                    unit.View.Transform.rotation,
                    Quaternion.LookRotation(targetVector, Vector3.up),
                    unit.Config.SmoothRotation);
            }
        }
    }
}