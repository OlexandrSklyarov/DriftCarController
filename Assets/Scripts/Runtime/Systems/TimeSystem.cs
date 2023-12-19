using Leopotam.EcsLite;
using UnityEngine;

namespace SA.Game 
{
    public sealed class TimeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private TimeService _ts;

        public void Init(IEcsSystems systems)
        {
            _ts = systems.GetShared<SharedData>().TimeService;           
        }

        public void Run(IEcsSystems systems)
        {
            _ts.Time = Time.time;
            _ts.UnscaledTime = Time.unscaledTime;
            _ts.DeltaTime = Time.deltaTime;
            _ts.FixedDeltaTime = Time.fixedDeltaTime;
            _ts.UnscaledDeltaTime = Time.unscaledDeltaTime;
        }
    }
}