using System;

namespace SA.Game
{
    [Serializable]
    public class PlayerSaveData
    {
        public SoundData Sound;

        public PlayerSaveData()
        {
            Sound = new SoundData() 
            {
                Master = 0.5f,
                Music = 0.5f,
                Ambience = 0.5f,
                Sfx = 0.5f,
            };
        }

        #region Data
        [Serializable]
        public class SoundData
        {     
            public float Master;
            public float Music;
            public float Ambience;
            public float Sfx;
        }
        #endregion

        public override string ToString()
        {
            return $"{Sound.Master} + {Sound.Music} + {Sound.Ambience} {Sound.Sfx}";
        }
    }
}