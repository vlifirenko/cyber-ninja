using CyberNinja.Models.Enums;
using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(menuName = "Config/Input", fileName = "Input")]
    public class InputConfig : ScriptableObject
    {
        public EInputType defaultInputType = EInputType.Keyboard;
    }
}