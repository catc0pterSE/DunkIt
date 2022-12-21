using Camera;
using Infrastructure;
using Infrastructure.Input;
using Infrastructure.ServiceManagement;
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

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();
        
        private Vector3 CameraRelativeDirection =>
            _cameraTargetTracker.CalculateCameraRelativeDirection(InputService.MovementDirection);

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
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

            Quaternion toRotation = Quaternion.LookRotation(CameraRelativeDirection, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}