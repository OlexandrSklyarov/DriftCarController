using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace SA.Game
{
    public class AudioService
    {
        private List<EventInstance> _eventInstances = new();
        private Bus _masterBus;
        private Bus _musicBus;
        private Bus _ambienceBus;
        private Bus _sfxBus;

        public AudioService()
        {
            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        }        

        public void PlayOneShot(EventReference sound, Vector3 pos)
        {
            RuntimeManager.PlayOneShot(sound, pos);
        }

        public EventInstance Create2DAudioInstance(EventReference sound)
        {
            var instance = RuntimeManager.CreateInstance(sound);
            _eventInstances.Add(instance);
            return instance;
        }

        public void Clear()
        {
            foreach(var instance in _eventInstances)
            {
                instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instance.release();
            }
        }

        public void SetMasterVolume(float value) => SetBusVolume(ref _masterBus, value);

        public void SetMusicVolume(float value) => SetBusVolume(ref _musicBus, value);

        public void SetAmbienceVolume(float value) => SetBusVolume(ref _ambienceBus, value);
        
        public void SetSFXVolume(float value) => SetBusVolume(ref _sfxBus, value);

        public void SetBusVolume(ref Bus bus, float value) => bus.setVolume(Mathf.Clamp01(value));
    }
}