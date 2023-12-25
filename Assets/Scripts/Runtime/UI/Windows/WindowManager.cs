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
                CreateAndShow<T>(type);

                return;
            }

            if (window.IsOpened) return;

            Show(window);
        }

        private void CreateAndShow<T>(Type type) where T : IWindow
        {
            IWindow window = CreateWindow<T>();
            window.Init(_resolver);
            _cashedWindows.Add(type, window);
            Show(window);
        }

        private void Show(IWindow window)
        {
            _openedWindows.Push(window);
            window.OnCloseEvent += OnWindowClose;
            window.Show();
        }

        private void OnWindowClose(IWindow window)
        {
            var lastOpenedWindow = _openedWindows.Peek();
            if (lastOpenedWindow != window)
            {
                throw new ArgumentException($"The window ({window.Name}) being closed is not the same as the last ({lastOpenedWindow.Name}) one added to the stack.");
            }

            window.OnCloseEvent -= OnWindowClose;
            _openedWindows.Pop();
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
                var window = _openedWindows.Pop();
                window.OnCloseEvent -= OnWindowClose;
                window.Hide();
            }
        }
    }
}