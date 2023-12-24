using System;
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
               
                AddAccel(ref input, ref engine);              
                AddSteer(ref input, ref engine);              
                AddBrake(ref input, ref engine); 

                TryApplyWheelPrm(ref engine);
            }
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

            foreach(var w in engine.EngineRef.Wheels)
            {
                if (!w.IsFront) continue;

                var steerAngle = input.Horizontal * config.Steer.Sensitivity * config.Steer.MaxSteerAngle;  
                w.Wheel.steerAngle = Mathf.Clamp(steerAngle, -config.Steer.MaxSteerAngle, config.Steer.MaxSteerAngle); 
            }
        }

        private void AddAccel(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;             
                           
            foreach(var w in engine.EngineRef.Wheels)
            {
                if (!w.IsDriveWheel) continue;  

                var normSpeed = engine.RealSpeed / config.SpeedLimit;
                var motorPower = config.MotorPower * config.Gear.AccelMultiplierCurve.Evaluate(normSpeed);
                
                w.Wheel.motorTorque = (engine.RealSpeed < config.SpeedLimit) ?  
                    motorPower * input.Vertical * _time.FixedDeltaTime : 0f;   

            }    
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