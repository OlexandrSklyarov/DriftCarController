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

            //engine
            ref var target = ref world.GetPool<FollowTargetComponent>().Add(entity);
            target.TargetRef = carView.transform;
        }
    }
}