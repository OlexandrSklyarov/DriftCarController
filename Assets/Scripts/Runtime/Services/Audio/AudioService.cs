using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace SA.Game
{
    public class AudioService : IDisposable
    {
        public float MasterVolume => _dataManager.Data.Sound.Master;
        public float MusicVolume => _dataManager.Data.Sound.Master;
        public float AmbienceVolume => _dataManager.Data.Sound.Master;
        public float SFXVolume => _dataManager.Data.Sound.Master;

        private readonly List<EventInstance> _eventInstances = new();
        private readonly PersistentDataManager _dataManager;
        private Bus _masterBus;
        private Bus _musicBus;
        private Bus _ambienceBus;
        private Bus _sfxBus;

        public AudioService(PersistentDataManager dataManager)
        {
            _dataManager = dataManager;
            
            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");

            SetMaster(MasterVolume); 
            SetMusic(MusicVolume); 
            SetAmbience(AmbienceVolume); 
            SetSFX(SFXVolume); 
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

            _eventInstances.Clear();

            Debug.Log("AudioService clear");
        }

        public void SetMaster(float value) 
        {
            SetBusVolume(ref _masterBus, value);
            _dataManager.Data.Sound.Master = value;
            _dataManager.Save();
        }

        public void SetMusic(float value) 
        {
            SetBusVolume(ref _musicBus, value);
            _dataManager.Data.Sound.Music = value;
            _dataManager.Save();
        }

        public void SetAmbience(float value) 
        { 
            SetBusVolume(ref _ambienceBus, value);
            _dataManager.Data.Sound.Ambience = value;
            _dataManager.Save();
        }
        
        public void SetSFX(float value) 
        {
            SetBusVolume(ref _sfxBus, value);  
            _dataManager.Data.Sound.Sfx = value;
            _dataManager.Save();  
        } 

        public void SetBusVolume(ref Bus bus, float value) => bus.setVolume(Mathf.Clamp01(value));

        public void Dispose()
        {
            Clear();
        }
    }
}