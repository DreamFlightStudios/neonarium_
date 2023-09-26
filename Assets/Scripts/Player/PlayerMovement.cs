using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        [Header("Float")]
        [SerializeField] private float _walkSpeed = 5;
        [SerializeField] private float _runSpeed = 8;
        [SerializeField] private float _jumpForce = 5;
        [SerializeField] private float _gravity = -9.81f;

        private InputSystem _inputSystem;
        private CharacterController _characterController;
        private Camera _playerCamera;
        private PlayerVfx _vfx;
        private Vector3 _velocity;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient && IsOwner) Init();
        }

        private void Init()
        {
            _inputSystem = new InputSystem();
            _inputSystem.Player.Jump.performed += _ => Jump();
            _inputSystem.Player.Enable();

            _vfx = new PlayerVfx(GetComponent<Animator>());
            
            _characterController = GetComponent<CharacterController>();
            _playerCamera = Camera.main;
            
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (!IsOwner) return;
            
            bool isSprint = _inputSystem.Player.Sprint.IsPressed();
            
            Vector2 direction = _inputSystem.Player.Move.ReadValue<Vector2>();
            direction *= isSprint ? _runSpeed : _walkSpeed;
            Vector3 move = Quaternion.Euler(0, _playerCamera.transform.localEulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
            _velocity = new Vector3(move.x, _velocity.y, move.z);
            
            _vfx.Move(_velocity.x != 0 || _velocity.z != 0 ? isSprint ? 1 : 0.5f : 0);
            _vfx.Fall(_velocity.y);
            
            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (!IsOwner || !Application.isFocused) return;

            _playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z);
            transform.rotation = Quaternion.Euler(0, _playerCamera.transform.rotation.y, 0);
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;
            
            if (_characterController.isGrounded) _velocity.y = -0.1f;
            else _velocity.y += _gravity * Time.fixedDeltaTime;
        }

        private void Jump() { if (_characterController.isGrounded) _velocity.y = _jumpForce; }

        public override void OnNetworkDespawn()
        {
            _inputSystem.Player.Jump.performed -= _ => Jump();
            base.OnNetworkDespawn();
        }
    }
}