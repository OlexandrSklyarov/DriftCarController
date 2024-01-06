using Leopotam.EcsLite;

namespace SA.Game
{
    public sealed class CarDashbordPanelSystem : IEcsInitSystem, IEcsRunSystem
    {
        private Hud _hud;
        private EcsPool<CarEngineComponent> _enginePool;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _hud = systems.GetShared<SharedData>().HUD;

            var world = systems.GetWorld();

            _enginePool = world.GetPool<CarEngineComponent>();
            
            _filter = world.Filter<CarEngineComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var engine = ref _enginePool.Get(ent);

                _hud.CarDashbord.SpeedDisplay.SetValue(engine.RealSpeed);
                _hud.CarDashbord.RPMDisplay.SetValue(engine.EngineRPM);
                _hud.CarDashbord.SetGearValue(engine.GearIndex+1);
            }
        }
    }
}