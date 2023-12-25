using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SA.Game
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainConfig _mainConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("GameLifetimeScope Configure...");

            builder.RegisterInstance(_mainConfig);

            builder.Register<WindowManager>(Lifetime.Singleton).WithParameter(_mainConfig.Window);
            builder.Register<AudioService>(Lifetime.Singleton);

            RegisterInput(builder);
            
            builder.RegisterComponentInHierarchy<EcsStartup>();
        }

        private void RegisterInput(IContainerBuilder builder)
        {
#if UNITY_ANDROID

            builder.Register<IInputService, KeyboardInput>(Lifetime.Singleton);

#elif UNITY_EDITOR

            builder.Register<IInputService, KeyboardInput>(Lifetime.Singleton);

#endif
        }
    }
}