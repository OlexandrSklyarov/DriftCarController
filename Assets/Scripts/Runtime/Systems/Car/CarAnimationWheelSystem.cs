using Leopotam.EcsLite;

namespace SA.Game
{
    public sealed class CarAnimationWheelSystem : IEcsInitSystem, IEcsRunSystem
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

                foreach(var w in engine.EngineRef.Wheels)
                {
                    w.Wheel.GetWorldPose(out var pos, out var rot);
                    w.TransformRef.SetPositionAndRotation(pos, rot);
                }             
            }
        }
    }
}