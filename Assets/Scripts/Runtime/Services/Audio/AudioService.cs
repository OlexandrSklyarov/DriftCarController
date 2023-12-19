using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace SA.Game
{
    public class AudioService
    {
        private List<EventInstance> _eventInstances = new();

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
    }
}