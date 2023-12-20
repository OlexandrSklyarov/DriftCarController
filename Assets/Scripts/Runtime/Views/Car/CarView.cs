using System;
using UnityEngine;

namespace SA.Game
{
    public sealed class CarView : MonoBehaviour, ICarEngine
    {       
        [field: SerializeField] public Rigidbody RB {get; private set;}
        [field: SerializeField] public CarConfig Config {get; private set;}
        [field: Space, SerializeField] public WheelData[] Wheels {get; private set;}

        [Space, SerializeField] private Transform _centerOfMass;

        private void Awake()
        {
            RB.centerOfMass = _centerOfMass.localPosition;

            ApplyWheelPrm();
        }

        private void ApplyWheelPrm()
        {
            Array.ForEach(Wheels, w =>
            {
                w.Wheel.forwardFriction = SetWheelPrm(w.Wheel.forwardFriction, Config.WheelConfig.Forward);                
                w.Wheel.sidewaysFriction = SetWheelPrm(w.Wheel.sidewaysFriction, Config.WheelConfig.Side);                
            });
        }

        private WheelFrictionCurve SetWheelPrm(WheelFrictionCurve sidewaysFriction, WheelFriction config)
        {
            sidewaysFriction.extremumSlip = config.ExtremumSlip;
            sidewaysFriction.extremumValue = config.ExtremumValue;
            sidewaysFriction.asymptoteSlip = config.AsymptoteValue;
            sidewaysFriction.asymptoteValue = config.AsymptoteValue;
            sidewaysFriction.stiffness = config.Stiffness;

            return sidewaysFriction;
        }

        #region Data
        [System.Serializable]
        public struct WheelData
        {
            public WheelCollider Wheel;
            public Transform TransformRef;
            public bool IsFront;
        }
        #endregion
    }
}
