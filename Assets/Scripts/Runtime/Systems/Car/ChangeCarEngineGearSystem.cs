using Leopotam.EcsLite;
using UnityEngine;

namespace SA.Game
{
    public sealed class ChangeCarEngineGearSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<CarEngineComponent> _enginePool;
        private EcsPool<ChangeGearEvent> _gearEventPool;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _enginePool = world.GetPool<CarEngineComponent>();  
            _gearEventPool = world.GetPool<ChangeGearEvent>();  

            _filter = world.Filter<CarEngineComponent>()
                .Inc<ChangeGearEvent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var engine = ref _enginePool.Get(ent);
                ref var evt = ref _gearEventPool.Get(ent);

                engine.GearIndex += evt.Value;
                engine.GearIndex = Mathf.Clamp(engine.GearIndex, 0, engine.EngineRef.Config.Gear.Values.Length-1); 
            }
        }
    }
}