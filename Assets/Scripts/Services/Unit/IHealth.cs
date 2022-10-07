using CyberNinja.Ecs.Components.Unit;

namespace CyberNinja.Services.Unit
{
    public interface IHealth
    {
        public HealthComponent GetHealth(int entity);

        public void UpdateHealth(int entity, float value);
    }
}