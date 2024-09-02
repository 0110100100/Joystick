using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour, Joystick.IPlayerActions
    {
        [SerializeField] private float moveSpeed = 0.5f;
        [SerializeField] private float dashSpeed = 5f;
        [SerializeField] private float dashTime = 1f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float jumpForce = 10f;

        private Joystick _joystick;
        private CharacterController _player;
        private Animator _animator;

        private bool _isDashing = false;
        private Vector2 _moveInput;

        private void Awake()
        {
            _joystick = new Joystick();
            _player = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            MovePlayer();
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
                Debug.Log("perfomed");
                _moveInput = context.ReadValue<Vector2>();
            }
            else if (context.canceled)
            {
                Debug.Log("canceled");
                _moveInput = Vector2.zero;
            }
        }

        private void MovePlayer()
        {
            Vector3 moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;

            if (moveDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
            }

            _player.Move(moveDirection * moveSpeed * Time.deltaTime);

            float speed = moveDirection.magnitude;
            _animator.SetFloat("Speed", speed);
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!_isDashing && context.started)
            {                
                StartCoroutine(DashPlayer());
            }
        }

        IEnumerator DashPlayer()
        {
            _isDashing = true;
            float startTime = Time.time;

            while (Time.time < startTime + dashTime)
            {
                _player.Move(transform.forward * dashSpeed * Time.deltaTime);
                yield return null;
            }

            _isDashing = false;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _animator.SetTrigger("Jump");
                _player.Move(Vector3.up * jumpForce * Time.deltaTime);
            }
        }
    }
}

