using Camera;
using Infrastructure;
using Services.Input;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class InputPlayerMover : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 4;
        [SerializeField] private float _rotationSpeed = 400;
        [SerializeField] private float _gravityModifier = 1;

        private CameraTargetTracker _cameraTargetTracker;

        private CharacterController _characterController;
        private IInputService _inputService;

        private Vector3 CameraRelativeDirection =>
            _cameraTargetTracker.CalculateCameraRelativeDirection(_inputService.MovementDirection);

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _inputService = Game.InputService;
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
        }

        public void SetTargetTracker(CameraTargetTracker targetTracker)
        {
            _cameraTargetTracker = targetTracker;
        }

        private void Move()
        {
            Vector3 movement = CameraRelativeDirection + Physics.gravity * _gravityModifier;
            _characterController.Move(movement * (Time.deltaTime * _movementSpeed));
        }

        private void Rotate()
        {
            if (CameraRelativeDirection == Vector3.zero)
                return;

            //   transform.forward = CameraRelativeDirection; //TODO: choose realization

            Quaternion toRotation = Quaternion.LookRotation(CameraRelativeDirection, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}