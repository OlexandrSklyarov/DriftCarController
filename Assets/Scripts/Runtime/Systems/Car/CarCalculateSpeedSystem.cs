using Leopotam.EcsLite;
using UnityEngine;

namespace SA.Game
{
    public sealed class CarCalculateSpeedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<CarEngineComponent> _enginePool;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _enginePool = world.GetPool<CarEngineComponent>();  

            _filter = world.Filter<CarEngineComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var engine = ref _enginePool.Get(ent);

                CalcRPM(ref engine);
                CalcSpeed(ref engine);  
            }
        }

        private void CalcRPM(ref CarEngineComponent engine)
        {
            var count = 0;
            var rpm = 0f;

            foreach (var w in engine.EngineRef.Wheels)
            {
                rpm += w.Wheel.rpm;
                count++;
            }
            
            engine.RPM = (count > 0) ? rpm / count : 0f;
        }

        private void CalcSpeed(ref CarEngineComponent engine)
        {
            var val = new Vector2(engine.EngineRef.RB.linearVelocity.x, engine.EngineRef.RB.linearVelocity.z).magnitude;
            engine.RealSpeed = val * 2.236936f;
        }
    }
}