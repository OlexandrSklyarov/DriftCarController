using Leopotam.EcsLite;
using UnityEngine;

namespace SA.Game
{
    public sealed class CarInitSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();

            var carView = UnityEngine.Object.Instantiate
            (
                data.MainConfig.CarViewPrefab,
                data.SceneData.CarSpawnPoint.position,
                Quaternion.identity
            );

            var world = systems.GetWorld();
            var entity = world.NewEntity();

            //input
            world.GetPool<PlayerInputComponent>().Add(entity);

            //engine
            ref var engine = ref world.GetPool<CarEngineComponent>().Add(entity);
            engine.EngineRef = carView;

            //target
            world.GetPool<FollowCameraTargetTag>().Add(entity);

            //drift
            world.GetPool<CarDriftComponent>().Add(entity);

            //audio
            ref var audio = ref world.GetPool<CarAudioComponent>().Add(entity);
            audio.MoveSFX = data.AudioService.Create2DAudioInstance(carView.Config.Audio.EngineMoveSfx);
        }
    }
}