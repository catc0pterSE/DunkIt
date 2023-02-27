using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.CutsceneStates;
using Gameplay.StateMachine.States.Gameplay;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;

namespace Gameplay.StateMachine.Transitions
{
    public class GameplayStateToDunkStateTransition : ITransition
    {
        private readonly GameplayState _gameplayState;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private IInputService _inputService;

        public GameplayStateToDunkStateTransition(GameplayState gameplayState,
            GameplayLoopStateMachine gameplayLoopStateMachine, IInputService inputService)
        {
            _inputService = inputService;
            _gameplayState = gameplayState;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable() =>
            _inputService.DunkButtonUp += MoveToDunkState;

        public void Disable() =>
            _inputService.DunkButtonUp -= MoveToDunkState;


        private void MoveToDunkState()
        {
            if (_gameplayState.ControlledPlayer.IsInDunkZone == false)
                return;

            _gameplayLoopStateMachine.Enter<DunkState, PlayerFacade>(_gameplayState.ControlledPlayer);
        }
    }
}