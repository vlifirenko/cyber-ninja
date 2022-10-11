using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Config;
using CyberNinja.Views.Unit;
using UnityEngine;

namespace CyberNinja.Services.Unit
{
    public interface IUnitService : IPlayer, IState, IHealth
    {
        public int CreateUnit(UnitView view);
        
        public UnitComponent GetUnit(int entity);

        public void AddDamage(int entity, float damage, Transform damageOrigin);

        public void UpdateEnergy(int entity, float value);

        public void TryDash(int entity, AbilityConfig abilityConfig, bool hit, Vector3 hitVector);
    }
}