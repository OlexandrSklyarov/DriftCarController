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
            }
        }

        private void AddBrake(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;

            foreach(var w in engine.EngineRef.Wheels)
            {                
                w.Wheel.brakeTorque = (input.IsBrake) ?
                    config.BrakeMultiplier * config.Brake * _time.DeltaTime : 0f;
            }
        }

        private void AddSteer(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;

            foreach(var w in engine.EngineRef.Wheels)
            {
                if (!w.IsFront) continue;

                var steerAngle = input.Horizontal * config.MaxSteerAngle;
                w.Wheel.steerAngle = Mathf.Lerp(w.Wheel.steerAngle, steerAngle, config.SteerTime);
            }
        }

        private void AddAccel(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;

            foreach(var w in engine.EngineRef.Wheels)
            {
                w.Wheel.motorTorque = input.Vertical * config.MovementMultiplier * config.Accel * _time.DeltaTime;
            }
        }
    }
}