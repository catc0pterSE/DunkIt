using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;

namespace Gameplay.StateMachine.Transitions
{
    public class GameplayStateToPassTransition : ITransition
    {
        private readonly GameplayState _gameplayState;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        private IInputService _inputService;

        public GameplayStateToPassTransition(GameplayState gameplayState,
            GameplayLoopStateMachine gameplayLoopStateMachine, IInputService inputService)
        {
            _inputService = inputService;
            _gameplayState = gameplayState;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable() =>
            _inputService.PassButtonDown += MoveToPassState;


        public void Disable() =>
            _inputService.PassButtonDown -= MoveToPassState;


        private void MoveToPassState()
        {
            if (_gameplayState.ControlledPlayer.IsPassPossible == false)
                return;

            _gameplayLoopStateMachine.Enter<PassState, PlayerFacade>(_gameplayState.ControlledPlayer);
        }
    }
}