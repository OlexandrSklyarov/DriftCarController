using UnityEngine;

namespace SA.Game
{
    public interface IInputService : IService
    {
        Vector2 Movement {get;}
        bool IsBreak {get;}

        void ActiveInput();
        void DisableInput();
    }
}