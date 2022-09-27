using UnityEngine;

namespace CyberNinja.Config
{
    [CreateAssetMenu(menuName = "Config/Audio", fileName = "Audio")]
    public class AudioConfig : ScriptableObject
    {
        [Range(0, 100)] public int masterVolume = 70;
        [Space]
        [Range(0, 100)] public int musicVolume = 30;
        [Range(0, 100)] public int effectsVolume = 100;
        [Range(0, 100)] public int environmentVolume = 100;
        public bool isMusicStart = true;
    }
}