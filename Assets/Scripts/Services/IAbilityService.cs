using CyberNinja.Models.Ability;

namespace CyberNinja.Services
{
    public interface IAbilityService
    {
        public void CreateAbility(AbilityItem abilityItem, int ownerEntity);

        public (bool, int) TryActivateAbility(int slotIndex, int unitEntity);
    }
}