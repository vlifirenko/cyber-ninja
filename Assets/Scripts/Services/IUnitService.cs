using CyberNinja.Config;
using CyberNinja.Ecs.Components;
using CyberNinja.Ecs.Components.Unit;
using CyberNinja.Models.Ability;
using CyberNinja.Models.Enums;
using CyberNinja.Services.Impl;
using CyberNinja.Views;
using CyberNinja.Views.Unit;
using UnityEngine;

namespace CyberNinja.Services
{
    public interface IUnitService
    {
        public int CreateUnit(UnitView view);

        public void AddDamage(int entity, float damage, Transform damageOrigin);

        public bool AddState(int entity, EUnitState state, float value = 0);

        public bool HasState(int entity, EUnitState state);

        public bool RemoveState(int entity, EUnitState state);

        public bool IsPlayer(int entity);

        public HealthComponent GetHealth(int entity);

        public void UpdateHealth(int entity, float value);
        
        public void UpdateEnergy(int entity, float value);

        public UnitComponent GetUnit(int entity);

        public void TryDash(int entity, AbilityConfig abilityConfig, bool hit, Vector3 hitVector);
    }
}