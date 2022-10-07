using CyberNinja.Models.Enums;

namespace CyberNinja.Services.Unit
{
    public interface IState
    {
        public bool AddState(int entity, EUnitState state, float value = 0);

        public bool HasState(int entity, EUnitState state);

        public bool RemoveState(int entity, EUnitState state);
    }
}