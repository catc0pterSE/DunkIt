using Infrastructure.ServiceManagement;
using Utility.Static;

namespace Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ServiceRegistrator _serviceRegistrator;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, ServiceRegistrator serviceRegistrator)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _serviceRegistrator = serviceRegistrator;
        }
        
        public void Enter()
        {
            _serviceRegistrator.RegisterServices();
            _sceneLoader.LoadScene(SceneNames.Initial, onLoaded: EnterLoadLevel);
        }
        
        public void Exit()
        {
            
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>(SceneNames.Scene);
        }
    }
}