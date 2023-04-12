using System;
using System.Collections.Generic;
using Gameplay.Character.Player.MonoBehaviour.Events;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using Infrastructure.Input.InputService;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour.Brains.LocalControlled
{
    public class LocalControlledBrain : SwitchableComponent
    {
        [SerializeField] private PlayerMover _mover;
        [SerializeField] private PlayerEventLauncher _eventLauncher;

        private Transform _gameplayCamera;
        private IInputService _inputService;

        private Dictionary<LocalAction, Action> _typedActions;

        public void Initialize(Transform gameplayCamera, IInputService inputService)
        {
            _gameplayCamera = gameplayCamera;
            _inputService = inputService;
            
            _typedActions = new Dictionary<LocalAction, Action>
            {
                [LocalAction.Dunk] = SubscribeOnDunkInput,
                [LocalAction.Move] = SubscribeMoveOnInput,
                [LocalAction.Pass] = SubscribeOnPassInput,
                [LocalAction.Throw] = SubscribeOnThrowInput,
                [LocalAction.ChangePlayer] = SubscribeOnChangePlayerInput,
                [LocalAction.Rotate] = SubscribeRotateOnInput
             };
        }

        private void OnDisable()
        {
            UnsubscribeFromDunkInput();
            UnsubscribeFromPassInput();
            UnsubscribeFromThrowInput();
            UnsubscribeFromChangePlayerInput();
            UnsubscribeMoveFromInput();
            UnsubscribeRotateFromInput();
        }

        public void Enable(LocalAction[] actionsToEnable)
        {
            base.Enable();
            actionsToEnable.Map(action => _typedActions[action].Invoke());
        }

        private void SubscribeMoveOnInput() =>
            _inputService.MovementInputReceived += MoveWithInput;

        private void UnsubscribeMoveFromInput() =>
            _inputService.MovementInputReceived -= MoveWithInput;
        
        private void SubscribeRotateOnInput() =>
            _inputService.MovementInputReceived += RotateWithInput;

        private void UnsubscribeRotateFromInput() =>
            _inputService.MovementInputReceived -= RotateWithInput;

        private void SubscribeOnThrowInput() =>
            _inputService.ThrowButtonDown += _eventLauncher.InitiateThrow;

        private void SubscribeOnPassInput() =>
            _inputService.PassButtonDown += _eventLauncher.InitiatePass;

        private void SubscribeOnDunkInput() =>
            _inputService.DunkButtonDown += _eventLauncher.InitiateDunk;

        private void SubscribeOnChangePlayerInput() =>
            _inputService.ChangePlayerButtonDown += _eventLauncher.InitiateChangePlayer;

        private void UnsubscribeFromThrowInput() =>
            _inputService.ThrowButtonDown -= _eventLauncher.InitiateThrow;

        private void UnsubscribeFromPassInput() =>
            _inputService.PassButtonDown -= _eventLauncher.InitiatePass;

        private void UnsubscribeFromDunkInput() =>
            _inputService.DunkButtonDown -= _eventLauncher.InitiateDunk;

        private void UnsubscribeFromChangePlayerInput() =>
            _inputService.ChangePlayerButtonDown -= _eventLauncher.InitiateChangePlayer;

        private void MoveWithInput(Vector2 inputDirection) =>
            _mover.Move(GetCameraRelativeDirection(inputDirection));
        
        private void RotateWithInput(Vector2 inputDirection) =>
            _mover.Rotate(GetCameraRelativeDirection(inputDirection));

        private Vector3 GetCameraRelativeDirection(Vector2 inputDirection)
        {
            Vector3 direction = _gameplayCamera.TransformDirection(inputDirection.x, 0, inputDirection.y);
            direction.y = 0;
            direction.Normalize();
            return direction;
        }
    }
}