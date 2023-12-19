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

                Debug.Log($"RPM [{engine.EngineRef.Wheels[0].Wheel.rpm}] speed [{engine.EngineRef.RB.velocity.magnitude}] torque [{engine.EngineRef.Wheels[0].Wheel.brakeTorque}]");          
                Debug.Log($"Drift points [{Mathf.Abs(Vector3.Dot(engine.EngineRef.RB.velocity.normalized, engine.EngineRef.RB.transform.right))}]");          
            }
        }

        private void AddBrake(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;

            foreach(var w in engine.EngineRef.Wheels)
            {                
                w.Wheel.brakeTorque = (input.IsBrake) ? config.Brake : 0f;
            }
        }

        private void AddSteer(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;

            foreach(var w in engine.EngineRef.Wheels)
            {
                if (!w.IsFront) continue;

                var angleMult = 1f - Mathf.Clamp(engine.EngineRef.RB.velocity.magnitude / config.SpeedThreshold, 0f, 0.95f);
                var maxAngle = config.MaxSteerAngle * angleMult;
                var steerAngle = input.Horizontal * maxAngle;
                w.Wheel.steerAngle = Mathf.Lerp(w.Wheel.steerAngle, steerAngle, config.SteerTime);
            }
        }

        private void AddAccel(ref PlayerInputComponent input, ref CarEngineComponent engine)
        {
            var config = engine.EngineRef.Config;

            foreach(var w in engine.EngineRef.Wheels)
            {
                w.Wheel.motorTorque = input.Vertical * config.Accel;
            }
        }
    }
}