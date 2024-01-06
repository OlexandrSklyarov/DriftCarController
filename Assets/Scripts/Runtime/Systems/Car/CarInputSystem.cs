using Leopotam.EcsLite;

namespace SA.Game
{
    public sealed class CarInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private IInputService _inputService;
        private EcsWorld _world;
        private EcsPool<PlayerInputComponent> _inputPool;
        private EcsFilter _openMenuEventFilter;
        private EcsFilter _changeGearEventFilter;
        private EcsPool<OpenMenuEvent> _openMenuEventPool;
        private EcsPool<ChangeGearEvent> _changeGearEventPool;
        private EcsFilter _inputFilter;

        public void Init(IEcsSystems systems)
        {
            _inputService = systems.GetShared<SharedData>().InputService;
           
            _world = systems.GetWorld();

            _inputPool = _world.GetPool<PlayerInputComponent>();
            _openMenuEventPool = _world.GetPool<OpenMenuEvent>();
            _changeGearEventPool = _world.GetPool<ChangeGearEvent>();
            
            _inputFilter = _world.Filter<PlayerInputComponent>().End();
            _openMenuEventFilter = _world.Filter<OpenMenuEvent>().End();
            _changeGearEventFilter = _world.Filter<ChangeGearEvent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            ClearEvents();

            foreach (var ent in _inputFilter)
            {
                ref var input = ref _inputPool.Get(ent);

                var movement = _inputService.Movement;
                input.Vertical = movement.y;
                input.Horizontal = movement.x;

                input.IsBrake = _inputService.IsBreak;

                //change gear
                if (_inputService.IsIncreaseGear)
                {
                    _changeGearEventPool.Add(ent).Value = 1;
                }
                else if (_inputService.IsDecreaseGear)
                {
                    _changeGearEventPool.Add(ent).Value = -1;
                }

                //menu event
                if (_inputService.IsOpenMenu)
                {
                    _openMenuEventPool.Add(_world.NewEntity());
                }
            }
        }

        private void ClearEvents()
        {            
            foreach (var ent in _openMenuEventFilter) 
                _openMenuEventPool.Del(ent);
            
            foreach (var ent in _changeGearEventFilter) 
                _changeGearEventPool.Del(ent);
        }
    }
}