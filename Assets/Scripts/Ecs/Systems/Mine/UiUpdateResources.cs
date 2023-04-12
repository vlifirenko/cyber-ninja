using System.Text;
using CyberNinja.Models;
using CyberNinja.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using TMPro;

namespace CyberNinja.Ecs.Systems.Mine
{
    public class UiUpdateResources : IEcsRunSystem
    {
        private EcsCustomInject<GameData> _gameData;

        [EcsUguiNamed(UiConst.ResourcesText)] private TMP_Text _resourcesText;

        public void Run(IEcsSystems systems)
        {
            var sb = new StringBuilder();
            
            foreach (var item in _gameData.Value.playerResources.items)
                sb.Append($"{item.type}: {item.value}\n");

            _resourcesText.text = sb.ToString();
        }
    }
}