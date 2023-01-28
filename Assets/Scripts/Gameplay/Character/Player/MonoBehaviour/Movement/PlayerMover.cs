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
    }
}