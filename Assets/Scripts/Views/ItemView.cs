using CyberNinja.Views.Core;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CyberNinja.Views
{
    public class ItemView : AEntityView
    {
        [BoxGroup("Configs"), SerializeField] private float cost;
        [BoxGroup("Configs"), SerializeField] private float weight;
        [BoxGroup("Configs"), SerializeField] private float volume;

        public float Cost => cost;
        public float Weight => weight;
        public float Volume => volume;
    }
}