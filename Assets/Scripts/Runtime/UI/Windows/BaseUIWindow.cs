using UnityEngine;
using UnityEngine.UI;

namespace SA.Game
{
    public abstract class BaseUIWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _closeButton;

        public bool IsOpened => gameObject.activeInHierarchy && gameObject.activeSelf;

        private void Awake() 
        {
            _closeButton.onClick.AddListener(() => Hide());

            OnInit();
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
    }
}