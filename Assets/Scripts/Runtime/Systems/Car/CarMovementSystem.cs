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

            foreach (var w in engine.EngineRef.Wheels)
            {
                var brakeMultiplier = (w.IsFront) ? 0.7f : 0.3f;
                var brakeValue = (input.IsBrake) ? 1f : 0f;
                w.Wheel.brakeTorque = brakeValue * config.Brake * brakeMultiplier;

                if (!input.IsBrake && input.Vertical == 0 && engine.RealSpeed > 0f)
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

                var slipAngle = Vector3.Angle
                (
                    engine.EngineRef.RB.transform.forward, 
                    engine.EngineRef.RB.velocity - engine.EngineRef.RB.transform.forward
                );

                if (slipAngle > engine.EngineRef.Config.Drift.AutoSteeringAngleThreshold)
                {
                    steerAngle += Vector3.SignedAngle
                    (
                        engine.EngineRef.RB.transform.forward,
                        engine.EngineRef.RB.velocity + engine.EngineRef.RB.transform.forward,
                        Vector3.up
                    );
                }
                 
                w.Wheel.steerAngle = Mathf.Clamp(steerAngle, -config.MaxSteerAngle, config.MaxSteerAngle);             
            }
        }

        private void AddAccel(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;  

            foreach(var w in engine.EngineRef.Wheels)
            {
                if (!w.IsDriveWheel) continue;               

                w.Wheel.motorTorque = (engine.RealSpeed < config.SpeedLimit) ?
                    input.Vertical * config.Accel : 0f;  

                Debug.Log($"w.Wheel.motorTorque  {w.Wheel.name} {w.Wheel.motorTorque}");              
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