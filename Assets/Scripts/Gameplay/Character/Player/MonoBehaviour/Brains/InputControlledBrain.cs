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

        private Vector3 InputDirection => new Vector3(InputService.InputDirection.x, 0, InputService.InputDirection.y);

        private Vector3 GetCameraRelativeDirection()
        {
            Vector3 direction = _gameplayCamera.transform.TransformDirection(InputDirection);
            direction.y = 0;
            direction.Normalize();
            return direction;
        }

        private void Update()
        {
                _playerMover.Move(GetCameraRelativeDirection());
        }

        public void SetCamera(Transform gameplayCamera)
        {
            _gameplayCamera = gameplayCamera;
        }
    }
}