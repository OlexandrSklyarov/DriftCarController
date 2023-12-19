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
        [field: SerializeField, Min(1f)] public float SpeedThreshold {get; private set;} = 100f;
    }
}