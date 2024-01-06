using UnityEngine;

namespace SA.Game
{
    public class KeyboardInput : IInputService
    {
        Vector2 IInputService.Movement => _inputAction.Player.Movement.ReadValue<Vector2>();
        bool IInputService.IsBreak => _inputAction.Player.Break.ReadValue<float>() > 0f;
        bool IInputService.IsOpenMenu => _inputAction.Player.Menu.WasPressedThisFrame();
        bool IInputService.IsIncreaseGear => _inputAction.Player.IncGear.WasPressedThisFrame();
        bool IInputService.IsDecreaseGear => _inputAction.Player.DecGear.WasPressedThisFrame();

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