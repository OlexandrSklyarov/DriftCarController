using System;
using UnityEngine;

namespace SA.Game
{
    public class PersistentDataManager : IDisposable
    {
        public PlayerSaveData Data => _data;

        private PlayerSaveData _data;

        private const string PLAYER_DATA_KEY = "PLAYER_DATA_KEY";

        public PersistentDataManager()
        {
            LoadData();
        }

        private void LoadData()
        {
            Debug.Log("LOAD...");
            var str = PlayerPrefs.GetString(PLAYER_DATA_KEY, string.Empty);

            if (str == string.Empty)
            {
                _data = new PlayerSaveData();
                return;
            }

            _data = JsonUtility.FromJson<PlayerSaveData>(str);
        }

        public void Save()
        {
            var str = JsonUtility.ToJson(_data);
            PlayerPrefs.SetString(PLAYER_DATA_KEY, str);

            Debug.Log("SAVE...");
        }

        public void Dispose()
        {
            Save();
        }
    }
    
}