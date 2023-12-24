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
        [field: SerializeField, Min(1f)] public float SteerSmoothTime {get; private set;} = 10f;             
        [field: SerializeField] public AnimationCurve SteerCurve {get; private set;} = AnimationCurve.Linear(0f, 60f, 80f, 0.2f);
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
        [field: SerializeField, Min(0.1f)] public float AutoSteeringAngleThreshold {get; private set;} = 60f;
    }

    [Serializable]
    public class Gear
    {
        [field: SerializeField, Min(0.1f)] public float RedLineRPM {get; private set;} = 6500f;
        [field: SerializeField, Min(0.1f)] public float IdleRPM {get; private set;} = 800f;
        [field: SerializeField, Min(0.1f)] public float IdleRPMMultiplier {get; private set;} = 0.8f;
        [field: SerializeField, Min(0.1f)] public float IncreaseRPM {get; private set;} = 5500f;
        [field: SerializeField, Min(0.1f)] public float DecreaseRPM {get; private set;} = 3300f;
        [field: SerializeField, Min(0.1f)] public float ChangeRPMTime {get; private set;} = 3f;
        [field: SerializeField, Min(0.1f)] public float DifferentialRatio {get; private set;} = 4f;
        [field: SerializeField] public float[] GearRatios {get; private set;} = new float[] {3f, 2.5f, 2f, 1.5f, 1f, 0.8f};
        [field: SerializeField] public AnimationCurve RpmCurve {get; private set;} = AnimationCurve.Linear(0f, 0f, 0.8f, 1f);
    }
}