using UnityEngine;

namespace SA.Game
{
    [CreateAssetMenu(fileName = "CarConfig", menuName = "SO/Car/CarConfig")]
    public sealed class CarConfig : ScriptableObject
    {
        [field: SerializeField, Min(1f)] public float MovementMultiplier {get; private set;} = 600f; 
        [field: SerializeField, Min(1f)] public float BrakeMultiplier {get; private set;} = 300f; 
        [field: SerializeField, Min(1f)] public float Accel {get; private set;} = 30f; 
        [field: SerializeField, Min(1f)] public float Brake {get; private set;} = 50f; 
        [field: SerializeField, Min(1f)] public float MaxSteerAngle {get; private set;} = 30f; 
        [field: SerializeField, Min(0.1f)] public float TurnSensitivity {get; private set;} = 1f;
        [field: SerializeField, Min(0.1f)] public float SteerTime {get; private set;} = 0.6f;
    }
}