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
            _lateUpdateSystems = new EcsSystems (_world, sharedData);

            _updateSystems
            
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Add(new TimeSystem())
                .Add(new CarInitSystem())
                .Add(new CarInputSystem())
                .Add(new CarAnimationWheelSystem())
                .Init ();

            _lateUpdateSystems
            
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Add(new CarMovementSystem())
                .Init ();    
        }

        private void Update () 
        {
            _updateSystems?.Run ();
        }

        private void LateUpdate () 
        {
            _lateUpdateSystems?.Run ();
        }

        private void OnDestroy () 
        {
            _updateSystems?.Destroy ();
            _updateSystems = null;

            _lateUpdateSystems?.Destroy ();
            _lateUpdateSystems = null;
                        
            _world?.Destroy ();
            _world = null;            
        }
    }
}