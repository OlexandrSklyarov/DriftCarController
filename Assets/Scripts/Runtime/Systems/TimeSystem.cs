using Leopotam.EcsLite;

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
            _ts.OnUpdate();
        }
    }
}