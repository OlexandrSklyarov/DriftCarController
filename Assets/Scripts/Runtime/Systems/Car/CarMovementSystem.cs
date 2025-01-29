using System;
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
        private TimeService _time;

        public void Init(IEcsSystems systems)
        {
            _time = systems.GetShared<SharedData>().TimeService;
            
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
               
                AddMoveDownForce(ref engine);              
                CalculateTotalPower(ref input, ref engine);              
                AddAccel(ref engine);              
                AddSteer(ref input, ref engine);              
                AddBrake(ref input, ref engine); 

                TryApplyWheelPrm(ref engine);
            }
        }        

        private void AddMoveDownForce(ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;
            var rb = engine.EngineRef.RB;
            rb.AddForce(-rb.transform.up * rb.linearVelocity.magnitude * config.MoveDownForce);
        }

        private void AddBrake(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;           

            foreach (var w in engine.EngineRef.Wheels)
            {
                var brakeMultiplier = (w.IsFront) ? 0.7f : 0.3f;
                var brakeValue = (input.IsBrake) ? 1f : 0f;

                w.Wheel.brakeTorque = brakeValue * config.Brake * brakeMultiplier;

                if (input.Vertical == 0)
                {
                    w.Wheel.brakeTorque = config.Brake * 0.1f;
                }
            }
        }        

        private void AddSteer(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;

            var forwardSpeed = Vector3.Dot(engine.EngineRef.RB.transform.forward, engine.EngineRef.RB.linearVelocity);
            engine.SpeedFactor = Mathf.InverseLerp(0, config.SpeedLimit, forwardSpeed); 
            var currentSteerRange = Mathf.Lerp(config.Steer.MaxAngle, config.Steer.AngleAtMaxSpeed, engine.SpeedFactor);
                
            foreach(var w in engine.EngineRef.Wheels)
            {
                if (!w.IsFront) continue;  

                w.Wheel.steerAngle = input.Horizontal * config.Steer.Sensitivity * currentSteerRange;
            }
        }

        private void AddAccel(ref CarEngineComponent engine)
        {            
            foreach(var w in engine.EngineRef.Wheels)
            {
                if (!w.IsDriveWheel) continue;  
                
                w.Wheel.motorTorque = engine.TotalPower / engine.EngineRef.WheelDriveCount;   
            }    
        }      

        private void CalculateTotalPower(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;
            var curGear = config.Gear.Values[engine.GearIndex];

            engine.TotalPower = config.EnginePower.Evaluate(engine.EngineRPM) * curGear * input.Vertical;

            var velocity = 0f;

            engine.EngineRPM = Mathf.SmoothDamp
            (
                engine.EngineRPM, 
                1000f + Mathf.Abs(engine.RPM) * 3.6f * curGear,
                ref velocity,
                config.PowerSmoothTime
            );
        }  

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private void TryApplyWheelPrm(ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config.WheelConfig;

            Array.ForEach(engine.EngineRef.Wheels, w =>
            {
                w.ApplyWheelPrm(config);
            });
        }       
    }
}