using System;
using CyberNinja.Views.Ui;
using CyberNinja.Views.Unit;

namespace CyberNinja.Models
{
    [Serializable]
    public class Target
    {
        public UnitView unitView;
        public float distance;

        public event Action OnRemove;

        public void Remove() => OnRemove?.Invoke();
    }
}