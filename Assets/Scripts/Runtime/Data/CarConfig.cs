using System;
using FMODUnity;
using UnityEngine;

namespace SA.Game
{
    [CreateAssetMenu(fileName = "CarConfig", menuName = "SO/Car/CarConfig")]
    public sealed class CarConfig : ScriptableObject
    { 
        [field: SerializeField, Min(1f)] public float Accel {get; private set;} = 18000f; 
        [field: SerializeField, Min(1f)] public float Brake {get; private set;} = 50000f; 
        [field: SerializeField, Min(1f)] public float MaxSteerAngle {get; private set;} = 30f; 
        [field: SerializeField, Min(0.1f)] public float TurnSensitivity {get; private set;} = 1f;
        [field: SerializeField, Min(0.1f)] public float SteerTime {get; private set;} = 0.6f;
        [field: Tooltip("The speed threshold at which you lose control of wheel turning, the closer you get to the threshold the harder it is to control turning."), SerializeField, Min(1f)] 
        public float SteerControlThreshold {get; private set;} = 100f;

        [field: Space, SerializeField] public WheelConfig WheelConfig {get; private set;}
        
        [field: Space, SerializeField] public CarAudio Audio {get; private set;}
    }

    [Serializable]
    public class CarAudio
    {
        [field: SerializeField] public EventReference EngineMoveSfx {get; private set;}
        [field: SerializeField, Min(0.1f)] public float MinEnginePitch {get; private set;} = 0.2f;
        [field: SerializeField, Min(0.1f)] public float MaxEnginePitch {get; private set;} = 1f;
        [field: SerializeField, Min(0.1f)] public float MinSpeed {get; private set;} = 0.3f;
        [field: SerializeField, Min(0.1f)] public float MaxSpeed {get; private set;} = 40;
        [field: SerializeField, Min(0.1f)] public float SpeedAudioThreshold {get; private set;} = 50;
    }
}