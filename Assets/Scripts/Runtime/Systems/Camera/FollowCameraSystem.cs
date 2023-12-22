using Leopotam.EcsLite;
using UnityEngine;

namespace SA.Game
{
    public sealed class FollowCameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<CameraFollowComponent> _cameraPool;
        private EcsPool<CarEngineComponent> _enginePool;
        private EcsFilter _cameraFilter;
        private EcsFilter _targetFilter;
        private TimeService _time;

        public void Init(IEcsSystems systems)
        {
            _time = systems.GetShared<SharedData>().TimeService; 
            
            var world = systems.GetWorld();

            _cameraPool = world.GetPool<CameraFollowComponent>();   
            _enginePool = world.GetPool<CarEngineComponent>();   

            _cameraFilter = world.Filter<CameraFollowComponent>().End();
            _targetFilter = world.Filter<CarEngineComponent>()
                .Inc<FollowCameraTargetTag>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var camEnt in _cameraFilter)
            {
                ref var camera = ref _cameraPool.Get(camEnt);  

                foreach(var targetEnt in _targetFilter)
                {
                    ref var carEngine = ref _enginePool.Get(targetEnt); 

                    LookAtTarget(ref camera, ref carEngine);
                    MoveToTarget(ref camera, ref carEngine);
                }
            }
        }

        private void MoveToTarget(ref CameraFollowComponent camera, ref CarEngineComponent carEngine)
        {
            var trRef = carEngine.EngineRef.RB.transform;

            var targetPos = trRef.position + 
                trRef.forward * camera.Config.Offset.z +
                trRef.right * camera.Config.Offset.x +
                trRef.up * camera.Config.Offset.y;

            var camTR = camera.CameraRef.transform;
            camTR.position = Vector3.Lerp(camTR.position, targetPos, camera.Config.FollowSpeed * _time.FixedDeltaTime); 
        }

        private void LookAtTarget(ref CameraFollowComponent camera, ref CarEngineComponent carEngine)
        {
            var lookDir = carEngine.EngineRef.RB.transform.position - camera.CameraRef.transform.position;
            var rot = Quaternion.LookRotation(lookDir, Vector3.up);

            camera.CameraRef.transform.rotation = Quaternion.Lerp
            (
                camera.CameraRef.transform.rotation,
                rot,
                camera.Config.LookSpeed * _time.FixedDeltaTime
            );
        }
    }
}