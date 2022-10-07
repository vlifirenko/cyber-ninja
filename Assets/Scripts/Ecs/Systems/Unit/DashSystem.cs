using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Enums;
using CyberNinja.Services;
using CyberNinja.Services.Unit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Unit
{
    public class DashSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DashComponent>> _filter;
        private readonly EcsPoolInject<DashComponent> _dashPool;
        private readonly EcsCustomInject<IUnitService> _unitService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var dash = ref _dashPool.Value.Get(entity);
                dash.TimeLeft -= Time.deltaTime;

                if (dash.TimeLeft <= 0)
                {
                    _unitService.Value.RemoveState(entity, EUnitState.Knockout);
                    _unitService.Value.RemoveState(entity, EUnitState.Dash);
                }
                else
                {
                    _unitService.Value.AddState(entity, EUnitState.Knockout);

                    var view = _unitService.Value.GetUnit(entity).View;
                    if (!view.NavMeshAgent.enabled)
                        continue;

                    var speed = dash.Distance / dash.Time * Time.deltaTime;
                    var vector = dash.Vector;

                    vector *= speed;
                    vector.y = 0;
                    view.NavMeshAgent.Move(vector);
                }
            }
        }
    }
}