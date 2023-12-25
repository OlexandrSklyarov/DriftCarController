using UnityEngine;

namespace SA.Game
{
    public interface IInputService
    {
        Vector2 Movement {get;}
        bool IsBreak {get;}
        bool IsOpenMenu {get;}

        void ActiveInput();
        void DisableInput();
    }
}