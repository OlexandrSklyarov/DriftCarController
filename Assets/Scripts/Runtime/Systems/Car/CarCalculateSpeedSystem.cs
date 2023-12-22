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
                
                var circumFerence = 2.0f * Mathf.PI * rearWheel.radius; // Finding circumFerence 2 Pi R
                engine.SpeedOnKmh = (circumFerence * rearWheel.rpm) * 60 / 10000f; // finding kmh
                engine.SpeedOnMph = engine.SpeedOnKmh * 0.62f; // converting kmh to mph

                var mag = new Vector2(engine.EngineRef.RB.velocity.x, engine.EngineRef.RB.velocity.z).magnitude;
                engine.RealSpeed = mag;

                engine.RPM = rearWheel.rpm;

                UnityEngine.Debug.Log($"speed KMH: {Mathf.RoundToInt(engine.SpeedOnKmh)} speed MPH: {Mathf.RoundToInt(engine.SpeedOnMph)} RealSpeed: {Mathf.RoundToInt(engine.RealSpeed)}");

            }
        }
    }
}