using Leopotam.EcsLite;

namespace SA.Game
{
    public sealed class FollowCameraInitSystem : IEcsInitSystem
    {      
        public void Init(IEcsSystems systems)
        {
            var data =  systems.GetShared<SharedData>();
            
            var world = systems.GetWorld();           
            var entity = world.NewEntity();
            ref var followCamera = ref world.GetPool<CameraFollowComponent>().Add(entity);
            followCamera.Config = data.MainConfig.Camera;
            followCamera.CameraRef = data.SceneData.Camera; 
        }        
    }
}