using System;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;
using Random = UnityEngine.Random;

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

                ChangeGear(ref input, ref engine);
                CalculateTorque(ref input, ref engine);
                AddAccel(ref input, ref engine);              
                AddSteer(ref input, ref engine);              
                AddBrake(ref input, ref engine); 

                TryApplyWheelPrm(ref engine);
            }
        }

        private void ChangeGear(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;  

            engine.Clutch = Mathf.Lerp(engine.Clutch, 1f, _time.DeltaTime);       

            if (engine.NextChangeGearTime > _time.Time) return;

            engine.GearIndex = engine.NextGearIndex;

            if (input.Vertical > 0f && engine.RPM > config.Gear.IncreaseRPM)
            {
               SetGear(ref engine, config, 1, 1.5f);
            }
            else if (engine.RPM < config.Gear.DecreaseRPM)
            {
                SetGear(ref engine, config, -1, 0.5f); 
            }
        }

        private void SetGear(ref CarEngineComponent engine, CarConfig config, int gearValue, float changeDelay)
        {
            engine.NextGearIndex += gearValue;
            engine.NextGearIndex = Mathf.Clamp(engine.NextGearIndex, 0, config.Gear.GearRatios.Length - 1);
            engine.NextChangeGearTime = _time.Time + changeDelay;

            engine.Clutch = (engine.GearIndex == 0 || engine.GearIndex == config.Gear.GearRatios.Length - 1) ?
                engine.Clutch : 0f;            
        }

        private void CalculateTorque(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config; 
            var torque = 0f;            

            if (engine.Clutch < 0.1f)
            {
                engine.RPM = Mathf.Lerp
                (
                    engine.RPM,
                    Mathf.Max(config.Gear.IdleRPM, config.Gear.RedLineRPM * input.Vertical) + Random.Range(-50f, 50f),
                    _time.DeltaTime
                );
            }
            else
            {   
                var wheelRPMSum = engine.EngineRef.Wheels
                .Where(x => x.IsDriveWheel)
                .Average(x => x.Wheel.rpm);

                const float MAGIC_VALUE = 5252f;                

                var gearRatio = config.Gear.GearRatios[engine.GearIndex] * config.Gear.DifferentialRatio;
                engine.WheelRPM = Mathf.Abs(wheelRPMSum) * gearRatio;

                engine.RPM = Mathf.Lerp
                (
                    engine.RPM,
                    Mathf.Max(config.Gear.IdleRPM * config.Gear.IdleRPMMultiplier, engine.WheelRPM),
                    _time.DeltaTime * config.Gear.ChangeRPMTime
                );

                var curRPM = (engine.RPM <= 0f) ? float.MinValue : engine.RPM;

                torque = (config.Gear.RpmCurve.Evaluate(curRPM / config.Gear.RedLineRPM) * 
                    config.MotorPower / curRPM) * gearRatio * MAGIC_VALUE * engine.Clutch;
            }

            engine.CurrentTorque = torque;
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
                    w.Wheel.brakeTorque = config.Brake * 0.3f;
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
                if (engine.RealSpeed > config.SpeedLimit) continue;               

                w.Wheel.motorTorque = (engine.RealSpeed < config.SpeedLimit) ?  
                    engine.CurrentTorque * config.MotorPower : 0f;      
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