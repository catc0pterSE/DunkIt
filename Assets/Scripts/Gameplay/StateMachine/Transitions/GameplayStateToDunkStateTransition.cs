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

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();

        public GameplayStateToDunkStateTransition(GameplayState gameplayState,
            GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _gameplayState = gameplayState;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable()
        {
            InputService.DunkButtonDown += MoveToDunkState;
        }

        public void Disable()
        {
            InputService.DunkButtonDown -= MoveToDunkState;
        }

        private void MoveToDunkState()
        {
            if (_gameplayState.ControlledPlayer.IsInDunkZone == false)
                return;

            _gameplayLoopStateMachine.Enter<DunkState, PlayerFacade>(_gameplayState.ControlledPlayer);
        }
    }
}