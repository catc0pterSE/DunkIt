using Gameplay.Camera;
using Gameplay.Player.MonoBehaviour.Movement;
using Infrastructure.Input;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Player.MonoBehaviour.Brain
{
    public class InputControlledBrain : SwitchableComponent
    {
        [SerializeField] private PlayerMover _playerMover;
        
        private CameraTargetTracker _cameraTargetTracker;
        private IInputService _inputService;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();

        private Vector3 CameraRelativeDirection =>
            _cameraTargetTracker.CalculateCameraRelativeDirection(InputService.MovementDirection);

        private void FixedUpdate()
        {
            _playerMover.Move(CameraRelativeDirection);
        }

        public void SetTargetTracker(CameraTargetTracker targetTracker)
        {
            _cameraTargetTracker = targetTracker;
        }
    }
}