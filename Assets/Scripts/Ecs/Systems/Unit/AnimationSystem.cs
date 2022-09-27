using CyberNinja.Config;
using CyberNinja.Ecs.Components.Ai;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class AnimationSystem : IEcsRunSystem
    {
        private static readonly int hashDiffVectorX = Animator.StringToHash("diffVectorX");
        private static readonly int hashDiffVectorZ = Animator.StringToHash("diffVectorZ");
        private static readonly int hashSpeed = Animator.StringToHash("speed");
        private static readonly int hashDashTime = Animator.StringToHash("dashTime");

        private readonly EcsFilterInject<Inc<UnitComponent, MoveVectorComponent>, Exc<DeadComponent>> _filter;
        private readonly EcsCustomInject<UnitConfig> _characterConfig;
        private readonly EcsCustomInject<IUnitService> _unitService;
        private readonly EcsPoolInject<VectorsComponent> _vectorsPool;
        private readonly EcsPoolInject<MoveVectorComponent> _moveVectorPool;
        private readonly EcsPoolInject<VectorLookComponent> _vectorLookPool;
        private readonly EcsPoolInject<SpeedComponent> _speedPool;
        private readonly EcsPoolInject<DashComponent> _dashPool;
        private readonly EcsPoolInject<AiTargetComponent> _aiTargetPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var unit = _unitService.Value.GetUnit(entity);
                var speed = _speedPool.Value.Get(entity);
                var differenceValues = Vector2.zero;

                if (unit.View.RotateToLookVector)
                {
                    if (_vectorsPool.Value.Get(entity).IsActiveVectorLook)
                    {
                        var moveVector = _moveVectorPool.Value.Get(entity);
                        if (_vectorLookPool.Value.Has(entity) && moveVector.Value != Vector3.zero)
                        {
                            var vectorLook = _vectorLookPool.Value.Get(entity);
                            var angle = Quaternion.FromToRotation(moveVector.Value, vectorLook.Value).eulerAngles;
                            if (angle.z != 0)
                                angle = new Vector3(0, 180, 0);
                            
                            var finalVector = Quaternion.AngleAxis(angle.y, Vector3.up) * Vector3.forward;
                            finalVector = new Vector3(-finalVector.x, finalVector.y, finalVector.z);
                            
                            differenceValues = new Vector2(finalVector.x, finalVector.z);
                            differenceValues *= speed.SpeedCurrent / _characterConfig.Value.defaultMoveSpeed;
                        }
                        else
                        {
                            differenceValues = _dashPool.Value.Has(entity) ? new Vector2(0, 1) : new Vector2(0, 0);
                        }
                    }
                }
                else
                {
                    differenceValues = new Vector2(0, 1);
                }

                var animator = unit.View.Animator;
                var x = animator.GetFloat(hashDiffVectorX);
                var z = animator.GetFloat(hashDiffVectorZ);
                var lerp = unit.View.DiffVectorLerp;
                var lerpX = Mathf.Lerp(x, differenceValues.x, lerp);
                var lerpZ = Mathf.Lerp(z, differenceValues.y, lerp);
                var differenceVector = _aiTargetPool.Value.Has(entity)
                    ? new Vector3(-lerpX, 0, lerpZ)
                    : new Vector3(lerpX, 0, lerpZ);

                // dash timer
                float dashTime;
                if (_dashPool.Value.Has(entity))
                {
                    var dash = _dashPool.Value.Get(entity);
                    dashTime = 1 - dash.TimeLeft / dash.Time;
                }
                else
                {
                    dashTime = 1;
                }

                animator.SetFloat(hashSpeed, speed.SpeedCurrent);
                animator.SetFloat(hashDiffVectorX, differenceVector.x);
                animator.SetFloat(hashDiffVectorZ, differenceVector.z);
                animator.SetFloat(hashDashTime, dashTime);
            }
        }
    }
}