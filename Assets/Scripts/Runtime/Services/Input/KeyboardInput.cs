using UnityEngine;

namespace SA.Game
{
    public class KeyboardInput : IInputService
    {
        Vector2 IInputService.Movement => _inputAction.Player.Movement.ReadValue<Vector2>();
        bool IInputService.IsBreak => _inputAction.Player.Break.ReadValue<float>() > 0f;

        private Controls _inputAction;

        public KeyboardInput()
        {
            _inputAction = new Controls();
            _inputAction.Enable();
        }


        void IInputService.ActiveInput()
        {
            _inputAction.Enable();
        }

        void IInputService.DisableInput()
        {
            _inputAction.Disable();
        }        
    }
}