using System;
using FMODUnity;
using UnityEngine;

namespace SA.Game
{
    [CreateAssetMenu(fileName = "CarConfig", menuName = "SO/Car/CarConfig")]
    public sealed class CarConfig : ScriptableObject
    { 
        [field: SerializeField, Min(1f)] public float MotorPower {get; private set;} = 100f; 
        [field: SerializeField, Min(1f)] public float SpeedLimit {get; private set;} = 60f; 
        [field: SerializeField, Min(1f)] public float Brake {get; private set;} = 50000f; 
        [field: Space, SerializeField] public Steer Steer {get; private set;}     
        [field: Space, SerializeField] public Gear Gear {get; private set;}     
        [field: Space, SerializeField] public WheelConfig WheelConfig {get; private set;}
        [field: Space, SerializeField] public CarDrift Drift {get; private set;}
        [field: Space, SerializeField] public CarAudio Audio {get; private set;}
    }

    [Serializable]
    public class Steer
    {
        [field: SerializeField, Min(1f)] public float MaxSteerAngle {get; private set;} = 60f;             
        [field: SerializeField, Min(0.1f)] public float Sensitivity {get; private set;} = 1f;             
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

    [Serializable]
    public class Gear
    {
        [field: SerializeField] public AnimationCurve AccelMultiplierCurve {get; private set;} = AnimationCurve.Linear(0f, 3f, 1f, 1f);
    }    
}