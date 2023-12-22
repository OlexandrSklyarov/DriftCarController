using System;
using UnityEngine;

namespace SA.Game
{
    [CreateAssetMenu(fileName = "WheelConfig", menuName = "SO/Car/WheelConfig")]
    public sealed class WheelConfig : ScriptableObject
    {
        [field: SerializeField, Min(0f)] public float WheelDampingRate {get; private set;} = 1f;
        [field: SerializeField, Min(0f)] public float SuspensionDistance {get; private set;} = 0.8f;
        [field: SerializeField, Min(0f)] public float ForceAppPointDistance {get; private set;} = 0f;

        [field: Space, SerializeField] public SuspensionSpring SP {get; private set;} 
        [field: Space, SerializeField] public WheelPair Front {get; private set;} 
        [field: Space, SerializeField] public WheelPair Back {get; private set;} 
    }

    [Serializable]
    public class SuspensionSpring
    {
        [field: SerializeField, Min(1f)] public float Spring {get; private set;} = 90000f;
        [field: SerializeField, Min(1)] public int Damper {get; private set;} = 9000;
        [field: SerializeField, Range(0f, 1f)] public float TargetPosition {get; private set;} = 1f;
    }

    [Serializable]
    public class WheelPair
    {
        [field: SerializeField] public WheelFriction Forward {get; private set;} 
        [field: SerializeField] public WheelFriction Side {get; private set;} 
    }

    [Serializable]
    public class WheelFriction
    {
        [field: SerializeField] public float ExtremumSlip {get; private set;} = 0.4f;
        [field: SerializeField] public float ExtremumValue {get; private set;} = 1;
        [field: SerializeField] public float AsymptoteSlip {get; private set;} = 0.8f;
        [field: SerializeField] public float AsymptoteValue {get; private set;} = 0.5f;
        [field: SerializeField] public float Stiffness {get; private set;} = 2f;
    }
}