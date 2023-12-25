using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace SA.Game
{
    public class WindowManager
    {
        private readonly Dictionary<Type, IWindow> _cashedWindows = new();
        private readonly Stack<IWindow> _openedWindows = new();
        private readonly WindowConfig _config;
        private Transform _windowsRoot;

        [Inject] private IObjectResolver _resolver;

        public WindowManager(WindowConfig config)
        {
            _config = config;
        }

        public void OpenWindow<T>() where T : IWindow
        {   
            var type = typeof(T);

            if (!_cashedWindows.TryGetValue(type, out IWindow window))
            {
                window = CreateWindow<T>();
                _cashedWindows.Add(type, window);
            }

            if (window.IsOpened) 
            {
                Debug.Log($"window {type} opened...");
                return;
            }

            Show(window);
        }

        private void Show(IWindow window)
        {
            _openedWindows.Push(window);
            window.Show();
        }

        private IWindow CreateWindow<T>() where T : IWindow
        {
            var uiScene = SceneManager.GetSceneByName("UI");

            if (_windowsRoot == null)
            {
                _windowsRoot = new GameObject($"[Windows]").transform;
                SceneManager.MoveGameObjectToScene(_windowsRoot.gameObject, uiScene);
            }
           
            var prefab = _config.WindowPrefabs.First(x => x is T);
            var window = _resolver.Instantiate(prefab, _windowsRoot);        

            return window;
        }

        public void CloseAll()
        {
            while (_openedWindows.Count > 0)
            {
                _openedWindows.Pop().Hide();
            }
        }
    }
}