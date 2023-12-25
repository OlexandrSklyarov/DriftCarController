using Leopotam.EcsLite;

namespace SA.Game
{
    public sealed class CarInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IInputService _inputService;
        private EcsWorld _world;
        private EcsPool<PlayerInputComponent> _inputPool;
        private EcsFilter _eventFilter;
        private EcsPool<OpenMenuEvent> _eventPool;
        private EcsFilter _inputFilter;

        public void Init(IEcsSystems systems)
        {
            _inputService = systems.GetShared<SharedData>().InputService;
           
            _world = systems.GetWorld();

            _eventPool = _world.GetPool<OpenMenuEvent>();
            _inputPool = _world.GetPool<PlayerInputComponent>();
            
            _eventFilter = _world.Filter<OpenMenuEvent>().End();
            _inputFilter = _world.Filter<PlayerInputComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            //clear events
            foreach(var ent in _eventFilter)
            {
                _eventPool.Del(ent);
            }

            foreach(var ent in _inputFilter)
            {
                ref var input = ref _inputPool.Get(ent);

                var movement = _inputService.Movement;
                input.Vertical = movement.y;
                input.Horizontal = movement.x;

                input.IsBrake = _inputService.IsBreak;
                
                //menu event
                if (_inputService.IsOpenMenu)
                {
                    _eventPool.Add(_world.NewEntity());
                }
            }
        }
    }
}