using Leopotam.EcsLite;

namespace SA.Game
{
    public sealed class CarInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IInputService _inputService;
        private EcsPool<PlayerInputComponent> _inputPool;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _inputService = systems.GetShared<SharedData>().InputService;

            var world = systems.GetWorld();

            _inputPool = world.GetPool<PlayerInputComponent>();
            
            _filter = world.Filter<PlayerInputComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var input = ref _inputPool.Get(ent);

                var movement = _inputService.Movement;
                input.Vertical = movement.y;
                input.Horizontal = movement.x;

                input.IsBrake = _inputService.IsBreak;
            }
        }
    }
}