using System;
using VContainer;

namespace SA.Game
{
    public interface IWindow
    {
        string Name {get;}
        bool IsOpened {get;}
        event Action<IWindow> OnCloseEvent;

        void Init(IObjectResolver resolver);
        void Show();
        void Hide();
    }
}