using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace SA.Game
{
    public abstract class BaseUIWindow : MonoBehaviour, IWindow
    {
        public bool IsOpened => gameObject.activeInHierarchy && gameObject.activeSelf;
        public string Name => gameObject.name;

        [SerializeField] private Button _closeButton;
        protected IObjectResolver _resolver;

        public event Action<IWindow> OnCloseEvent;

        private void Awake() 
        {
            _closeButton.onClick.AddListener(CloseWindow);            
        }

        private void CloseWindow()
        {
            OnCloseEvent?.Invoke(this);
            Hide();
        }

        protected abstract void OnInit();

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Init(IObjectResolver resolver)
        {
            _resolver = resolver;
            OnInit();
        }
    }
}