using System;
using FMODUnity;
using UnityEngine;

namespace SA.Game
{
    [CreateAssetMenu(fileName = "CarConfig", menuName = "SO/Car/CarConfig")]
    public sealed class CarConfig : ScriptableObject
    { 
        [field: SerializeField] public AnimationCurve EnginePower {get; private set;}
        [field: SerializeField, Min(1f)] public float SpeedLimit {get; private set;} = 60f; 
        [field: SerializeField, Min(1f)] public float Brake {get; private set;} = 50000f; 
        [field: SerializeField, Min(1f)] public float MoveDownForce {get; private set;} = 50f;
        [field: SerializeField, Min(0.01f)] public float PowerSmoothTime {get; private set;} = 0.1f;
        [field: Space, SerializeField] public Steer Steer {get; private set;}     
        [field: Space, SerializeField] public Gear Gear {get; private set;}     
        [field: Space, SerializeField] public WheelConfig WheelConfig {get; private set;}
        [field: Space, SerializeField] public CarDrift Drift {get; private set;}
        [field: Space, SerializeField] public CarVfx VFX {get; private set;}
        [field: Space, SerializeField] public CarAudio Audio {get; private set;}
    }

    [Serializable]
    public class Steer
    {
        [field: SerializeField, Min(1f)] public float MaxAngle {get; private set;} = 30f;             
        [field: SerializeField, Min(1f)] public float AngleAtMaxSpeed {get; private set;} = 10f;             
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
        [field: SerializeField, Min(0.1f)] public float SlipAllowance {get; private set;} = 0.9f;
        [field: SerializeField, Min(0.1f)] public float SpeedThreshold {get; private set;} = 5f;
        [field: SerializeField, Min(0.1f)] public float MinDriftAngle {get; private set;} = 60f;
        [field: SerializeField, Min(0.1f)] public float MaxDriftAngle {get; private set;} = 120f;
        [field: SerializeField, Min(0.1f)] public float DisableDriftDelay {get; private set;} = 2f;
        [field: SerializeField, Min(1f)] public float MaxDriftPointsFactor {get; private set;} = 6f;
    }  

    [Serializable]
    public class CarVfx
    {
        [field: SerializeField, Min(0.1f)] public float MaxBreakingThreshold {get; private set;} = 40f;
        [field: SerializeField, Min(0.1f)] public float StartingSpeedThreshold {get; private set;} = 20f;
        [field: SerializeField, Min(0.1f)] public float StartingRPMThreshold {get; private set;} = 20f;
    }

    [Serializable]
    public class Gear
    {        
        [field: SerializeField] public float[] Values {get; private set;} = new float[]
        {
            2f, 1.7f, 1.3f, 0.9f, 0.4f
        };
    }    
}