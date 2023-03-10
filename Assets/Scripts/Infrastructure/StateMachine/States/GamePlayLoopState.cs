using Gameplay.StateMachine;
using Modules.StateMachine;

namespace Infrastructure.StateMachine.States
{
    public class GamePlayLoopState : IParameterState<GameplayLoopStateMachine>
    {
        private readonly GameStateMachine _gameStateMachine;

        public GamePlayLoopState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            gameplayLoopStateMachine.Run();
        }

        public void Exit()
        {
            
        }
    }
}