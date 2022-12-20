using Infrastructure.StateMachine;
using Services.Input;
namespace Infrastructure
{
    public class Game
    {
        public static IInputService InputService;
        private readonly GameStateMachine _stateMachine;

        public Game()
        {
            _stateMachine = new GameStateMachine(new SceneLoader());
        }

        public GameStateMachine StateMachine => _stateMachine;
    }
}