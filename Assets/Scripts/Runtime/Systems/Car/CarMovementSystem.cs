using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

namespace SA.Game
{
    public sealed class CarMovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<PlayerInputComponent> _inputPool;
        private EcsPool<CarEngineComponent> _enginePool;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _inputPool = world.GetPool<PlayerInputComponent>();
            _enginePool = world.GetPool<CarEngineComponent>();
            
            _filter = world.Filter<PlayerInputComponent>()
                .Inc<CarEngineComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var input = ref _inputPool.Get(ent);
                ref var engine = ref _enginePool.Get(ent);  

                AddAccel(ref input, ref engine);              
                AddSteer(ref input, ref engine);              
                AddBrake(ref input, ref engine); 

                TryApplyWheelPrm(ref engine);
            }
        }        

        private void AddBrake(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;

            foreach(var w in engine.EngineRef.Wheels)
            {                
                w.Wheel.brakeTorque = (input.IsBrake) ? config.Brake : 0f;

                if (!input.IsBrake && input.Vertical == 0 && engine.Speed > 0f)
                {
                    w.Wheel.brakeTorque = config.Brake * 0.01f;
                }
            }
        }

        private void AddSteer(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;

            foreach(var w in engine.EngineRef.Wheels)
            {
                if (!w.IsFront) continue;

                var steerAngle = input.Horizontal * config.SteerCurve.Evaluate(engine.RealSpeed);
                w.Wheel.steerAngle = Mathf.Clamp(steerAngle, -config.MaxSteerAngle, config.MaxSteerAngle);                
            }
        }

        private void AddAccel(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;          
            

            foreach(var w in engine.EngineRef.Wheels)
            {
                if (!w.IsDriveWheel) continue;
                if (engine.RealSpeed >= config.MaxVelocity) continue;

                w.Wheel.motorTorque = input.Vertical * config.Accel;                
            }                     
        }       

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void TryApplyWheelPrm(ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;

            foreach(var w in engine.EngineRef.Wheels)
            {
                if (w.IsFront)
                {
                    w.Wheel.forwardFriction = SetWheelPrm(w.Wheel.forwardFriction, config.WheelConfig.Front.Forward);                
                    w.Wheel.sidewaysFriction = SetWheelPrm(w.Wheel.sidewaysFriction, config.WheelConfig.Front.Side);
                }
                else
                {
                    w.Wheel.forwardFriction = SetWheelPrm(w.Wheel.forwardFriction, config.WheelConfig.Back.Forward);                
                    w.Wheel.sidewaysFriction = SetWheelPrm(w.Wheel.sidewaysFriction, config.WheelConfig.Back.Side);  
                }
            };
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
    }
}