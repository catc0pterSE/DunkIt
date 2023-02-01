using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.Movement
{
    public class PlayerMover : SwitchableComponent
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _movementSpeed = 4;
        [SerializeField] private float _rotationSpeed = 400;
        [SerializeField] private float _gravityModifier = 1;

        private Coroutine _rotatingToPoint;
        private Coroutine _movingToPoint;

        private Action Reached;
        private Action Oriented;

        public void Move(Vector3 movementDirection)
        {
            Rotate(movementDirection);
            movementDirection += GetGravity();

            _characterController.Move(movementDirection * (Time.deltaTime * _movementSpeed));
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

        public void RotateTo(Vector3 position)
        {
        }

        public void MoveToTo(Vector3 position)
        {
        }
    }
}