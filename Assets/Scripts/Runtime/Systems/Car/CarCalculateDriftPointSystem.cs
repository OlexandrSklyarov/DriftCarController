using Leopotam.EcsLite;
using UnityEngine;

namespace SA.Game
{
    public sealed class CarCalculateDriftPointSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<CarEngineComponent> _enginePool;
        private EcsPool<CarDriftComponent> _driftPool;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _enginePool = world.GetPool<CarEngineComponent>();  
            _driftPool = world.GetPool<CarDriftComponent>();  

            _filter = world.Filter<CarEngineComponent>()
                .Inc<CarDriftComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var engine = ref _enginePool.Get(ent);   
                ref var drift = ref _driftPool.Get(ent);   

                var forward = engine.EngineRef.RB.transform.forward;              
                var vel = new Vector3(engine.EngineRef.RB.velocity.x, 0f, engine.EngineRef.RB.velocity.z).normalized;  
                var origin = engine.EngineRef.RB.transform.position;
                var length = 30f;

                Debug.DrawLine(origin, origin + forward * length, Color.green);
                Debug.DrawLine(origin, origin + vel * length, Color.blue);

                drift.Angle = Vector3.Angle(forward, vel);
            }
        }        
    }
}