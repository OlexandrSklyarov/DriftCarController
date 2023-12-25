using Leopotam.EcsLite;

namespace SA.Game 
{
    public sealed class OpenMenuSystem : IEcsInitSystem, IEcsRunSystem
    {
        private WindowManager _windowManager;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _windowManager = systems.GetShared<SharedData>().WindowManager; 

            var world = systems.GetWorld();
            
            _filter = world.Filter<OpenMenuEvent>().End();          
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                _windowManager.OpenWindow<AudioSettingsWindow>();
            }
        }
    }
}