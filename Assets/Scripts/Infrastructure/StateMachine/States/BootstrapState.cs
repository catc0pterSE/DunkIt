using Infrastructure.Factory;
using Infrastructure.Input;
using Infrastructure.Provider;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;
using Utility.Static;

namespace Infrastructure.StateMachine.States
{
    public class BootstrapState : IParameterlessState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly Services _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, Services services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            RegisterServices();
        }
        
        public void Enter()
        {
            _sceneLoader.LoadScene(SceneNames.Initial, onLoaded: EnterLoadLevel);
        }
        
        public void Exit()
        {
            
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>(SceneNames.Scene);
        }
        
        private void RegisterServices()
        {
            _services.RegisterSingle<IInputService>(new SimpleInputService()); //TODO: for different platforms
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IGameObjectFactory>(new GameObjectFactory(Services.Container.Single<IAssetProvider>()));
        }
    }
}