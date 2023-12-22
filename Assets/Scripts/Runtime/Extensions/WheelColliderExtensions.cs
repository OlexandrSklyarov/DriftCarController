using UnityEngine;

namespace SA.Game
{
    public static class WheelColliderExtensions
    {        
        public static void ApplyWheelPrm(this ref CarView.WheelData w, WheelConfig config)
        {            
            w.Wheel.wheelDampingRate = config.WheelDampingRate;   
            w.Wheel.suspensionDistance = config.SuspensionDistance;   
            w.Wheel.forceAppPointDistance = config.ForceAppPointDistance;

            w.Wheel.suspensionSpring = SetSuspensionSpring
            (
                w.Wheel.suspensionSpring,
                config.SP.Spring,
                config.SP.Damper,
                config.SP.TargetPosition
            );  

            if (w.IsFront)
            {
                w.Wheel.forwardFriction = SetWheelPrm(w.Wheel.forwardFriction, config.Front.Forward);                
                w.Wheel.sidewaysFriction = SetWheelPrm(w.Wheel.sidewaysFriction, config.Front.Side);
            }
            else
            {
                w.Wheel.forwardFriction = SetWheelPrm(w.Wheel.forwardFriction, config.Back.Forward);                
                w.Wheel.sidewaysFriction = SetWheelPrm(w.Wheel.sidewaysFriction, config.Back.Side);  
            }           
        }

        private static JointSpring SetSuspensionSpring(JointSpring suspensionSpring, float spring, int damper, float targetPosition)
        {
            suspensionSpring.spring = spring;
            suspensionSpring.damper = damper;
            suspensionSpring.targetPosition = targetPosition;

            return suspensionSpring;
        }

        private static WheelFrictionCurve SetWheelPrm(WheelFrictionCurve sidewaysFriction, WheelFriction config)
        {
            sidewaysFriction.extremumSlip = config.ExtremumSlip;
            sidewaysFriction.extremumValue = config.ExtremumValue;
            sidewaysFriction.asymptoteSlip = config.AsymptoteValue;
            sidewaysFriction.asymptoteValue = config.AsymptoteValue;
            sidewaysFriction.stiffness = config.Stiffness;

            return sidewaysFriction;
        }
    }
}