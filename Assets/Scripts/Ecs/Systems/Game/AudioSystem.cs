using CyberNinja.Models;
using CyberNinja.Models.Config;
using CyberNinja.Views;
using FMOD.Studio;
using FMODUnity;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace CyberNinja.Ecs.Systems.Game
{
    public class AudioSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<AudioConfig> _audioConfig;
        private readonly EcsCustomInject<SceneView> _sceneView;
        private readonly EcsCustomInject<GameData> _gameData;

        private Bus _masterBus;
        private VCA _musicVca;
        private VCA _effectsVca;
        private VCA _environmentVca;
        private int _musicProgress;
        private bool _isMusicEnd;

        public void Init(IEcsSystems systems)
        {
            StopMusic();
            if (_audioConfig.Value.isMusicStart)
                PlayMusic();

            _masterBus = RuntimeManager.GetBus("bus:/");

            _musicVca = RuntimeManager.GetVCA("vca:/Music");
            _effectsVca = RuntimeManager.GetVCA("vca:/Effects");
            _environmentVca = RuntimeManager.GetVCA("vca:/Environment");
        }

        public void Run(IEcsSystems systems)
        {
            var masterVol = (float) _audioConfig.Value.masterVolume / 100;
            var musicVol = (float) _audioConfig.Value.musicVolume / 100;
            var effectVol = (float) _audioConfig.Value.effectsVolume / 100;
            var environmentVol = (float) _audioConfig.Value.environmentVolume / 100;

            if (_gameData.Value.isMasterMute)
                masterVol = 0;
            if (_gameData.Value.isMusicMute)
                musicVol = 0;
            if (_gameData.Value.isEffectsMute)
                effectVol = 0;
            if (_gameData.Value.isEnvironmentMute)
                environmentVol = 0;

            _masterBus.setVolume(masterVol);
            _musicVca.setVolume(musicVol);
            _effectsVca.setVolume(effectVol);
            _environmentVca.setVolume(environmentVol);

            RuntimeManager.StudioSystem.setParameterByName("parameter:/GLOBALS/MusicProgress", _musicProgress);
            RuntimeManager.StudioSystem.setParameterByName("parameter:/GLOBALS/MusicEnd", _isMusicEnd ? 1 : 0);
        }

        private void PlayMusic() => _sceneView.Value.FmodEventEmitter.Play();

        private void StopMusic() => _sceneView.Value.FmodEventEmitter.Stop();
    }
}