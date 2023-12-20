using System;
using UnityEngine;

namespace SA.Game
{
    [CreateAssetMenu(fileName = "WheelConfig", menuName = "SO/Car/WheelConfig")]
    public sealed class WheelConfig : ScriptableObject
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