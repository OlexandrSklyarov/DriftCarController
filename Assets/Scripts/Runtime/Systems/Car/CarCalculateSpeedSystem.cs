using System.Linq;
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

                var rearWheel = engine.EngineRef.Wheels.First(x => x.IsDriveWheel).Wheel;
                engine.Speed = rearWheel.radius * Mathf.PI * rearWheel.rpm * 60f / 1000f;

                var mag = new Vector2(engine.EngineRef.RB.velocity.x, engine.EngineRef.RB.velocity.z).magnitude;
                engine.RealSpeed = mag;

                UnityEngine.Debug.Log($"speed: {Mathf.RoundToInt(engine.Speed)} vel: {Mathf.RoundToInt(engine.RealSpeed)}");

            }
        }
    }
}