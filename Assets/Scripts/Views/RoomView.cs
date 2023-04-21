using System;
using System.Collections.Generic;
using CyberNinja.Models.Config;
using CyberNinja.Models.Enums;
using CyberNinja.Views.Core;
using UnityEngine;

namespace CyberNinja.Views
{
    public class RoomView : AView
    {
        [SerializeField] private Transform playerSpawn;
        [SerializeField] private Transform[] enemySpawnPoints;
        [SerializeField] private RoomConfig roomConfig;
        [SerializeField] private GameObject door;

        [SerializeField] private EMineCellState _level;
        [SerializeField] private EMineCircle _circle;
        [SerializeField] private int _index;

        public EMineCellState Level
        {
            get => _level;
            set => _level = value;
        }

        public EMineCircle Circle
        {
            get => _circle;
            set => _circle = value;
        }

        public int Index
        {
            get => _index;
            set => _index = value;
        }

        public Transform PlayerSpawn => playerSpawn;

        public RoomConfig RoomConfig => roomConfig;

        public Transform[] EnemySpawnPoints => enemySpawnPoints;
        
        public Dictionary<EEnemyType, int> EnemyKillMap { get; set; } = new Dictionary<EEnemyType, int>();

        public GameObject Door => door;
    }
}