using System;
using Cinemachine;
using UnityEngine;

namespace SA.Game
{
    [Serializable]
    public class SceneData
    {
        [field: SerializeField] public Transform CarSpawnPoint {get; private set;} 
        [field: SerializeField] public CinemachineVirtualCamera Camera {get; private set;} 
        
    }
}