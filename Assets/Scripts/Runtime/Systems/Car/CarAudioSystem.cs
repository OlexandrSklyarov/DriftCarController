using Leopotam.EcsLite;

namespace SA.Game
{
    public sealed class CarAudioSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<CarAudioComponent> _audioPool;
        private EcsPool<CarEngineComponent> _enginePool;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {           
            var world = systems.GetWorld();

            _audioPool = world.GetPool<CarAudioComponent>();
            _enginePool = world.GetPool<CarEngineComponent>();
            
            _filter = world.Filter<CarAudioComponent>()
                .Inc<CarEngineComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var audio = ref _audioPool.Get(ent);
                ref var engine = ref _enginePool.Get(ent);

                var speed = engine.RealSpeed;

                audio.CurrentSpeed = speed;
                audio.PitchFromCar = speed / engine.EngineRef.Config.Audio.SpeedAudioThreshold;               
                
                if (audio.CurrentSpeed < engine.EngineRef.Config.Audio.MinSpeed)
                {                    
                    SetEngineAudioPitch(ref audio, engine.EngineRef.Config.Audio.MinEnginePitch); 
                }

                if (audio.CurrentSpeed > engine.EngineRef.Config.Audio.MinSpeed &&
                    audio.CurrentSpeed < engine.EngineRef.Config.Audio.MaxSpeed)
                {
                    SetEngineAudioPitch(ref audio, engine.EngineRef.Config.Audio.MinEnginePitch + audio.PitchFromCar); 
                }

                if (audio.CurrentSpeed > engine.EngineRef.Config.Audio.MaxSpeed)
                {
                    SetEngineAudioPitch(ref audio, engine.EngineRef.Config.Audio.MaxEnginePitch); 
                }
            }
        }

        private void SetEngineAudioPitch(ref CarAudioComponent audio, float pitch)
        {    
            audio.MoveSFX.TryPlay();
            audio.MoveSFX.setPitch(pitch); 
        }
    }
}