using System;
using System.Collections;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.Movement
{
    public class PlayerMover : SwitchableComponent
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _rotationSpeed = 50;
        [SerializeField] private float _gravityModifier = 1;
        [SerializeField] private float _minAngle = 5;
        [SerializeField] private float _movementSpeed = 4;

        private Coroutine _rotating;

        public void Configure(float movementSpeed)
        {
            _movementSpeed = movementSpeed;
        }

        private void OnDisable()
        {
            StopRotationRoutine();
        }

        public void MoveLookingStraight(Vector3 movementDirection)
        {
            Rotate(movementDirection);

            Move(movementDirection);
        }

        public void MoveLookingAt(Vector3 movementDirection, Vector3 lookAt)
        {
            Vector3 positionProjection = new Vector3(lookAt.x, transform.position.y, lookAt.z);

            Rotate(positionProjection - transform.position);
            Move(movementDirection);
        }

        private void Move(Vector3 movementDirection)
        {
            StopRotationRoutine();
            movementDirection += GetGravity();

            _characterController.Move(movementDirection * (Time.deltaTime * _movementSpeed));
        }

        public void RotateTo(Vector3 position, Action callback = null)
        {
            Vector3 positionProjection = new Vector3(position.x, transform.position.y, position.z);

            StopRotationRoutine();

            _rotating = StartCoroutine(RotateToPosition(positionProjection, callback));
        }

        private void StopRotationRoutine()
        {
            if (_rotating != null)
                StopCoroutine(_rotating);
        }


        private Vector3 GetGravity()
        {
            return Physics.gravity * _gravityModifier;
        }

        private void Rotate(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }

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