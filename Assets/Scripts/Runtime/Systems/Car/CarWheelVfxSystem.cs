using Leopotam.EcsLite;
using UnityEngine;

namespace SA.Game
{
    public sealed class CarWheelVfxSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<CarEngineComponent> _enginePool;
        private EcsPool<PlayerInputComponent> _inputPool;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _enginePool = world.GetPool<CarEngineComponent>();
            _inputPool = world.GetPool<PlayerInputComponent>();

            _filter = world.Filter<CarEngineComponent>()
                .Inc<PlayerInputComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var engine = ref _enginePool.Get(ent);
                ref var input = ref _inputPool.Get(ent);
                
                BreakingParticle(ref engine, ref input);
                DriftParticles(ref engine, ref input);
            }
        }

        private void BreakingParticle(ref CarEngineComponent engine, ref PlayerInputComponent input)
        {    
            foreach (var w in engine.EngineRef.Wheels)
            {
                var isSkid = input.IsBrake && !w.IsFront && engine.RealSpeed > engine.EngineRef.Config.Drift.SpeedThreshold;

                w.SkidVfx.emitting = isSkid;

                if (isSkid)
                {
                    w.SmokeVfx.Emit(1);
                }
            }
        }

        private void DriftParticles(ref CarEngineComponent engine, ref PlayerInputComponent input) 
        {
            if (input.IsBrake) return;
            
            var wheelHits = new WheelHit[engine.EngineRef.Wheels.Length];
            var drift = engine.EngineRef.Config.Drift;

            for(int i = 0; i < engine.EngineRef.Wheels.Length; i++)
            {
                var w = engine.EngineRef.Wheels[i];
                w.Wheel.GetGroundHit(out wheelHits[i]);

                if (engine.RealSpeed > drift.SpeedThreshold)
                {    
                    if (Mathf.Abs(wheelHits[i].sidewaysSlip) + Mathf.Abs(wheelHits[i].forwardSlip) > drift.SlipAllowance)
                    {
                        w.SkidVfx.emitting = true; 
                    }
                }
                else
                {
                    w.SkidVfx.emitting = false;
                }
            }   
        }
    }
}