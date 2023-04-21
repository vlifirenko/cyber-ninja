using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class LobbyMinesView : AView
    {
        [SerializeField] private LobbyMine player;
        [SerializeField] private LobbyMine enemyPrefab;
        [SerializeField] private Transform enemyContainer;

        public LobbyMine Player => player;

        public LobbyMine EnemyPrefab => enemyPrefab;

        public Transform EnemyContainer => enemyContainer;
    }
}