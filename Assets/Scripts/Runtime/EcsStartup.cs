using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace SA.Game 
{
    sealed class EcsStartup : MonoBehaviour 
    {
        [SerializeField] private SceneData _sceneData;        

        private SharedData _sharedData;
        private EcsWorld _world;             
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _lateUpdateSystems;

        [Inject] private MainConfig _mainConfig;
        [Inject] private WindowManager _windowManager;
        [Inject] private AudioService _audioService;
        [Inject] private IInputService _inputService;   

        private async void Start()
        {     
            _sharedData = new SharedData()
            {
                SceneData = _sceneData,
                MainConfig = _mainConfig,
                InputService = _inputService,
                TimeService = new TimeService(),
                AudioService = _audioService,
                HUD = await MonoComponentExtensions.FindComponentAsync<Hud>(),
                WindowManager = _windowManager
            };

            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world, _sharedData);
            _fixedUpdateSystems = new EcsSystems(_world, _sharedData);
            _lateUpdateSystems = new EcsSystems(_world, _sharedData);

            RegisterSystems();               
        }

        private void RegisterSystems()
        {
            _updateSystems            
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Add(new TimeSystem())
                .Add(new CarInitSystem())
                .Add(new CarInputSystem())
                .Add(new ChangeCarEngineGearSystem())
                .Add(new FollowCameraInitSystem())
                .Add(new CarWheelVfxSystem())
                .Add(new CarCalculateDriftPointSystem())
                .Add(new CarAudioSystem())
                .Add(new CarCalculateSpeedSystem())
                
                .Add(new OpenMenuSystem())
                .Init ();

            _fixedUpdateSystems            
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Add(new CarMovementSystem())
                .Add(new CarAnimationWheelSystem())                
                .Add(new FollowCameraSystem())
                .Init ();

            _lateUpdateSystems            
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Add(new CarDashbordPanelSystem())
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