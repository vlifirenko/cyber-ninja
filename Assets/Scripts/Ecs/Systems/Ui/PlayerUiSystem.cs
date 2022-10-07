using CyberNinja.Events;
using CyberNinja.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CyberNinja.Ecs.Systems.Ui
{
    public class PlayerUiSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<CanvasView> _canvasView;

        public void Init(IEcsSystems systems)
        {
            UnitEventsHolder.OnHealthUpdate += (entity, health)
                => UpdatePlayerHealth(health.x, health.y);
            UnitEventsHolder.OnEnergyUpdate += (entity, energy)
                => UpdateEnergyHealth(energy.x, energy.y);
        }

        private void UpdatePlayerHealth(float currentHealth, float maxHealth)
        {
            var finalHealthScaleX = Mathf.Clamp01(currentHealth / maxHealth);
            _canvasView.Value.PlayerHealthBar.transform.localScale = new Vector3(finalHealthScaleX, 1, 1);

            var finalHealthValue = Mathf.Clamp(currentHealth, 0, maxHealth);
            _canvasView.Value.PlayerHealthText.text = finalHealthValue.ToString("F0") + "/" + maxHealth;
        }
        
        private void UpdateEnergyHealth(float currentEnergy, float maxEnergy)
        {
            var finalEnergyScaleX = Mathf.Clamp01(currentEnergy / maxEnergy);
            _canvasView.Value.PlayerEnergyBar.transform.localScale = new Vector3(finalEnergyScaleX, 1, 1);

            var finalEnergyValue = Mathf.Clamp(currentEnergy, 0, maxEnergy);
            _canvasView.Value.PlayerEnergyText.text = finalEnergyValue.ToString("F0") + "/" + currentEnergy;
        }
    }
}