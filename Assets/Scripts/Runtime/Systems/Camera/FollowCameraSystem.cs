using Leopotam.EcsLite;
using UnityEngine;

namespace SA.Game
{
    public sealed class FollowCameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<CameraFollowComponent> _cameraPool;
        private EcsPool<FollowTargetComponent> _targetPool;
        private EcsFilter _cameraFilter;
        private EcsFilter _targetFilter;
        private TimeService _time;

        public void Init(IEcsSystems systems)
        {
            _time = systems.GetShared<SharedData>().TimeService; 
            
            var world = systems.GetWorld();

            _cameraPool = world.GetPool<CameraFollowComponent>();   
            _targetPool = world.GetPool<FollowTargetComponent>();   


            _cameraFilter = world.Filter<CameraFollowComponent>().End();
            _targetFilter = world.Filter<FollowTargetComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var camEnt in _cameraFilter)
            {
                ref var camera = ref _cameraPool.Get(camEnt);  

                foreach(var targetEnt in _targetFilter)
                {
                    ref var target = ref _targetPool.Get(targetEnt);  

                    if (target.TargetRef == null) continue;

                    LookAtTarget(ref camera, ref target);
                    MoveToTarget(ref camera, ref target);
                }
            }
        }

        private void MoveToTarget(ref CameraFollowComponent camera, ref FollowTargetComponent target)
        {
            var targetPos = target.TargetRef.position + 
                target.TargetRef.forward * camera.Config.Offset.z +
                target.TargetRef.right * camera.Config.Offset.x +
                target.TargetRef.up * camera.Config.Offset.y;

            var camTR = camera.CameraRef.transform;
            camTR.position = Vector3.Lerp(camTR.position, targetPos, camera.Config.FollowSpeed * _time.FixedDeltaTime); 
        }

        private void LookAtTarget(ref CameraFollowComponent camera, ref FollowTargetComponent target)
        {
            var lookDir = target.TargetRef.position - camera.CameraRef.transform.position;
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