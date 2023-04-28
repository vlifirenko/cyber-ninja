using UnityEngine;

namespace CyberNinja.Models.Config
{
    [CreateAssetMenu(fileName = "Army", menuName = "Config/Army")]
    public class ArmyConfig : ScriptableObject
    {
        public string name;
        public UnitConfig unitConfig;
        public int startLevel;
        public int startExp;
        public int maxExp;
        public Color color;
    }
}