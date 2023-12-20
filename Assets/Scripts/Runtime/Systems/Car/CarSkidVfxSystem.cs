using Leopotam.EcsLite;

namespace SA.Game
{
    public sealed class CarSkidVfxSystem : IEcsInitSystem, IEcsRunSystem
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
            foreach(var ent in _filter)
            {
                ref var engine = ref _enginePool.Get(ent);  
                ref var input = ref _inputPool.Get(ent);  

                foreach(var w in engine.EngineRef.Wheels)
                {
                    w.SkidVfx.emitting = input.IsBrake && !w.IsFront;
                }             
            }
        }
    }
}