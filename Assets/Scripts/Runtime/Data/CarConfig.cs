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
        [field: SerializeField, Min(1f)] public float MaxSteerAngle {get; private set;} = 60f;             
        [field: SerializeField] public AnimationCurve SteerCurve {get; private set;} = AnimationCurve.Linear(0f, 60f, 80f, 0.2f);

        [field: Space, SerializeField] public WheelConfig WheelConfig {get; private set;}
        
        [field: Space, SerializeField] public CarDrift Drift {get; private set;}

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

    [Serializable]
    public class CarDrift
    {
        [field: SerializeField, Min(0.1f)] public float SlipAllowance {get; private set;} = 1.3f;
        [field: SerializeField, Min(0.1f)] public float SpeedThreshold {get; private set;} = 5f;
    }
}