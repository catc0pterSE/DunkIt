using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Gameplay.StateMachine.States.MinigameStates;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;

namespace Gameplay.StateMachine.Transitions
{
    public class GameplayStateToThrowStateTransition : ITransition
    {
        private readonly GameplayState _gameplayState;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private IInputService _inputService;

        private bool ThrowPossible =>
            _gameplayState.ControlledPlayer.OwnsBall && _gameplayState.ControlledPlayer.IsInThrowZone;

        public GameplayStateToThrowStateTransition(GameplayState gameplayState,
            GameplayLoopStateMachine gameplayLoopStateMachine, IInputService inputService)
        {
            _inputService = inputService;
            _gameplayState = gameplayState;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable() =>
            _inputService.ThrowButtonUp += MoveToThrowState;

        public void Disable() =>
            _inputService.ThrowButtonUp -= MoveToThrowState;

        private void MoveToThrowState()
        {
            if (ThrowPossible == false)
                return;

            _gameplayLoopStateMachine.Enter<ThrowState, PlayerFacade>(_gameplayState.ControlledPlayer);
        }
    }
}