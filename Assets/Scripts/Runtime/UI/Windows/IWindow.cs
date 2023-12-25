namespace SA.Game
{
    public interface IWindow
    {
        bool IsOpened {get;}
        
        void Show();
        void Hide();
    }
}