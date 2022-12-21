using Infrastructure.Input;
using Infrastructure.ServiceManagement;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _stateMachine;

        public Game()
        {
            _stateMachine = new GameStateMachine(
                new SceneLoader(),
                new ServiceRegistrator()
                );
        }

        public GameStateMachine StateMachine => _stateMachine;
    }
}