using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace SA.Game
{
    public class AudioSettingsWindow : BaseUIWindow
    {
        [Space, SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private Slider _ambienceVolume;
        [SerializeField] private Slider _sfxVolume;
        
        private AudioService _audioService;


        protected override void OnInit()
        {
            _audioService = _resolver.Resolve<AudioService>();

            _masterVolume.onValueChanged.AddListener((v) => _audioService.SetMaster(v));
            _musicVolume.onValueChanged.AddListener((v) => _audioService.SetMusic(v));
            _ambienceVolume.onValueChanged.AddListener((v) => _audioService.SetAmbience(v));
            _sfxVolume.onValueChanged.AddListener((v) => _audioService.SetSFX(v));

            _masterVolume.value = _audioService.MasterVolume;
            _musicVolume.value = _audioService.MusicVolume;
            _ambienceVolume.value = _audioService.AmbienceVolume;
            _sfxVolume.value = _audioService.SFXVolume;
        }
    }
}
