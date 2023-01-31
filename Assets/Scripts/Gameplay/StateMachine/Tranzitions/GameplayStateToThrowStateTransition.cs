﻿using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;

namespace Gameplay.StateMachine.Tranzitions
{
    public class GameplayStateToThrowStateTransition : ITransition
    {
        private readonly GameplayState _gameplayState;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private IInputService _inputService;

        private bool ThrowPossible =>
            _gameplayState.ControlledPlayer.OwnsBall && _gameplayState.ControlledPlayer.IsInThrowZone;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();

        public GameplayStateToThrowStateTransition(GameplayState gameplayState,
            GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _gameplayState = gameplayState;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable()
        {
            InputService.ThrowButtonPressed += MoveToThrowState;
        }

        public void Disable()
        {
            InputService.ThrowButtonPressed -= MoveToThrowState;
        }

        private void MoveToThrowState()
        {
            if (ThrowPossible == false)
                return;

            _gameplayLoopStateMachine.Enter<ThrowState, PlayerFacade>(_gameplayState.ControlledPlayer);
        }
    }
}