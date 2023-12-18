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

            data.SceneData.Camera.Follow = carView.transform;
            data.SceneData.Camera.LookAt = carView.transform;

            var world = systems.GetWorld();
            var entity = world.NewEntity();

            world.GetPool<PlayerInputComponent>().Add(entity);

            ref var engine = ref world.GetPool<CarEngineComponent>().Add(entity);
            engine.EngineRef = carView;
        }
    }
}