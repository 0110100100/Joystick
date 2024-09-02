using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 0.5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float dashSpeed = 5f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float jumpForce = 10f;

        private CharacterController _player;
        private Animator _animator;
        private Vector2 _moveInput;
        private bool _isDashing = false;

        private void Awake()
        {
            _player = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            MovePlayer();
        }

        public void ReceiveInput(Vector2 input)
        {
            _moveInput = input;
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

        public void Dash()
        {
            if (!_isDashing)
            {
                _isDashing = true;                
                StartCoroutine(DashCoroutine());
            }
        }

        private IEnumerator DashCoroutine()
        {
            float startTime = Time.time;
            while (Time.time < startTime + dashDuration)
            {
                _player.Move(transform.forward * dashSpeed * Time.deltaTime);
                yield return null;
            }
            _isDashing = false;
        }

        public void Jump()
        {
            _animator.SetTrigger("Jump");
            _player.Move(Vector3.up * jumpForce * Time.deltaTime);
        }
    }
}