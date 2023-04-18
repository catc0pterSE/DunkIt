using System;
using System.Collections;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour.Movement
{
    public class PlayerMover : SwitchableComponent
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _rotationSpeed = 50;
        [SerializeField] private float _gravityModifier = 1;
        [SerializeField] private float _minAngle = 10;
        [SerializeField] private float _movementSpeed = 4;
        [SerializeField] private float _jumpForce = 10;
        [SerializeField] private float _jumpHeight = 5;

        private Coroutine _rotateRoutine;
        private Coroutine _jumpRoutine;

        public void Configure(float movementSpeed)
        {
            _movementSpeed = movementSpeed;
        }

        private void OnEnable()
        {
            _characterController.Enable();
        }

        private void Update() =>
            ApplyGravity();


        private void OnDisable()
        {
            _characterController.Disable();
        }

        public void Move(Vector3 movementDirection)
        {
            StopRotationRoutine();

            _characterController.Move(movementDirection * (Time.deltaTime * _movementSpeed));
        }

        public void Jump()
        {
            if (_characterController.isGrounded == false)
                return;
            
            if (_jumpRoutine != null)
                StopCoroutine(_jumpRoutine);

            _jumpRoutine = StartCoroutine(JumpRoutine());
        }

        private IEnumerator JumpRoutine()
        {
            float startHeight = transform.position.y;
            while (transform.position.y < _jumpHeight - startHeight)
            {
                _characterController.Move(Vector3.up * (_jumpForce * Time.deltaTime));
                yield return null;
            }
        }

        public void Rotate(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }

        public void RotateTo(Vector3 position, Action callback = null)
        {
            Vector3 positionProjection = new Vector3(position.x, transform.position.y, position.z);

            StopRotationRoutine();

            _rotateRoutine = StartCoroutine(RotateToPosition(positionProjection, callback));
        }

        private void StopRotationRoutine()
        {
            if (_rotateRoutine != null)
                StopCoroutine(_rotateRoutine);
        }

        private void ApplyGravity() =>
            _characterController.Move(Physics.gravity * (_gravityModifier * Time.deltaTime));

        private IEnumerator RotateToPosition(Vector3 position, Action callback = null)
        {
            Vector3 direction = position - transform.position;

            while (Vector3.Angle(transform.forward, direction) > _minAngle)
            {
                Rotate(direction);
                yield return null;
            }

            callback?.Invoke();
        }
    }
}