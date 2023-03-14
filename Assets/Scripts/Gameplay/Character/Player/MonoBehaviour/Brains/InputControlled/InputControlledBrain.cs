using Gameplay.Character.Player.MonoBehaviour.Movement;
using Infrastructure.Input.InputService;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.Brains
{
    public abstract class InputControlledBrain: SwitchableComponent
    {
        [SerializeField] private PlayerMover _mover;
        [SerializeField] private PlayerFacade _host;
        
        private Transform _gameplayCamera;
        private IInputService _inputService;

        protected IInputService InputService => _inputService;
        protected PlayerFacade Host => _host;
        
        public void Initialize(Transform gameplayCamera, IInputService inputService)
        {
            _gameplayCamera = gameplayCamera;
            _inputService = inputService;
        }
        
        private Vector3 InputDirection => new Vector3(_inputService.MovementInput.x, 0, _inputService.MovementInput.y);

        private Vector3 GetCameraRelativeDirection()
        {
            Vector3 direction = _gameplayCamera.TransformDirection(InputDirection);
            direction.y = 0;
            direction.Normalize();
            return direction;
        }

        private void Update() =>
            _mover.MoveLookingStraight(GetCameraRelativeDirection());


       
            
    }
}