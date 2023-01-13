using Gameplay.Character.Player.MonoBehaviour.Movement;
using Infrastructure.Input;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.Brains
{
    public class InputControlledBrain : SwitchableComponent
    {
        [SerializeField] private PlayerMover _playerMover;
        
        private Transform _gameplayCamera;
        private IInputService _inputService;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();

        private Vector3 InputDirection => _inputService.InputDirection;

        private Vector3 CameraRelativeDirection =>
            _gameplayCamera.transform.TransformDirection(InputDirection.x, 0, InputDirection.y);

        private void FixedUpdate()
        {
            _playerMover.Move(CameraRelativeDirection);
        }

        public void SetCamera(Transform gameplayCamera)
        {
            _gameplayCamera = gameplayCamera;
        }
    }
}