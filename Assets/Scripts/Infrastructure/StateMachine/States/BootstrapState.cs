using Services.Input;
using Utility;

namespace Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }
        
        public void Enter()
        {
            RegisterServices();
            _sceneLoader.LoadScene(SceneNames.Initial, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>(SceneNames.Scene);
        }
        
        public void Exit()
        {
            
        }
        
        private void RegisterServices()
        {
            Game.InputService = RegisterInputService();
        }

        private IInputService RegisterInputService() //TODO: for different platforms
        {
            return new SimpleInputService();
        }
    }
}