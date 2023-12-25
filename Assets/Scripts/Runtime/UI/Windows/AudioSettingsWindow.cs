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
            Debug.Log("OnInit");
            _audioService = _resolver.Resolve<AudioService>();

            _masterVolume.SetValueWithoutNotify(_audioService.MasterVolume);
            _musicVolume.SetValueWithoutNotify(_audioService.MusicVolume);
            _ambienceVolume.SetValueWithoutNotify(_audioService.AmbienceVolume);
            _sfxVolume.SetValueWithoutNotify(_audioService.SFXVolume);

            _masterVolume.onValueChanged.AddListener((v) => _audioService.SetMaster(v));
            _musicVolume.onValueChanged.AddListener((v) => _audioService.SetMusic(v));
            _ambienceVolume.onValueChanged.AddListener((v) => _audioService.SetAmbience(v));
            _sfxVolume.onValueChanged.AddListener((v) => _audioService.SetSFX(v));
        }
    }
}
