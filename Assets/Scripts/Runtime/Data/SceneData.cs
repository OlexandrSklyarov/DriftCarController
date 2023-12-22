using System;
using UnityEngine;

namespace SA.Game
{
    [Serializable]
    public class SceneData
    {
        [field: SerializeField] public Transform CarSpawnPoint {get; private set;} 
        [field: SerializeField] public Camera Camera {get; private set;} 
        [field: SerializeField] public Hud HUD {get; private set;} 
    }
}