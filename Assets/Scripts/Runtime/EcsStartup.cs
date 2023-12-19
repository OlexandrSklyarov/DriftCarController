using Leopotam.EcsLite;
using UnityEngine;

namespace SA.Game 
{
    sealed class EcsStartup : MonoBehaviour 
    {
        [SerializeField] private SceneData _sceneData;
        [SerializeField] private MainConfig _config;

        private EcsWorld _world;        
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _lateUpdateSystems;

        private void Start () 
        {
            var sharedData = new SharedData()
            {
                SceneData = _sceneData,
                MainConfig = _config,
                Input = new KeyboardInput(),
                TimeService = new TimeService()
            };

            _world = new EcsWorld ();
            _updateSystems = new EcsSystems (_world, sharedData);
            _fixedUpdateSystems = new EcsSystems (_world, sharedData);
            _lateUpdateSystems = new EcsSystems (_world, sharedData);

            _updateSystems            
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Add(new TimeSystem())
                .Add(new CarInitSystem())
                .Add(new FollowCameraInitSystem())
                .Init ();

            _fixedUpdateSystems            
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Add(new CarInputSystem())
                .Add(new CarMovementSystem())
                .Add(new CarAnimationWheelSystem())                
                .Add(new FollowCameraSystem())
                .Init ();

            _lateUpdateSystems            
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init ();    
        }

        private void Update () 
        {
            _updateSystems?.Run ();
        }

        private void FixedUpdate () 
        {
            _fixedUpdateSystems?.Run ();
        }

        private void LateUpdate () 
        {
            _lateUpdateSystems?.Run ();
        }

        private void OnDestroy () 
        {
            _updateSystems?.Destroy ();
            _updateSystems = null;

            _fixedUpdateSystems?.Destroy ();
            _fixedUpdateSystems = null;

            _lateUpdateSystems?.Destroy ();
            _lateUpdateSystems = null;
                        
            _world?.Destroy ();
            _world = null;            
        }
    }
}