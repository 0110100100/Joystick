using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class PlayerInputHandler : MonoBehaviour, Joystick.IPlayerActions
    {
        private PlayerController _playerController;
        private Joystick _joystick;

        private void Awake()
        {
            _joystick = new Joystick();
            _playerController = GetComponent<PlayerController>();
        }
        private void OnEnable()
        {
            _joystick.Player.Enable();
            _joystick.Player.SetCallbacks(this);
        }

        private void OnDisable()
        {
            _joystick.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 moveInput = context.ReadValue<Vector2>();
                _playerController.ReceiveInput(moveInput);
                Debug.Log("move");
            }
            else if (context.canceled)
            {
                Debug.Log("canceled");
                Vector2 moveInput = Vector2.zero;
                _playerController.ReceiveInput(moveInput);
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _playerController.Dash();
                Debug.Log("dash");
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _playerController.Jump();
                Debug.Log("jump");
            }
        }
    }
}