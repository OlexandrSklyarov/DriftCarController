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
        
        [Inject] private AudioService _audioService;


        protected override void OnInit()
        {
            _masterVolume.onValueChanged.AddListener((v) => _audioService.SetMasterVolume(v));
            _musicVolume.onValueChanged.AddListener((v) => _audioService.SetMusicVolume(v));
            _ambienceVolume.onValueChanged.AddListener((v) => _audioService.SetAmbienceVolume(v));
            _sfxVolume.onValueChanged.AddListener((v) => _audioService.SetSFXVolume(v));
        }
    }
}
