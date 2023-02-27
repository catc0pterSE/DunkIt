using Infrastructure.Factory;
using Infrastructure.Input;
using Infrastructure.Input.InputService;
using Infrastructure.Provider;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;
using Utility.Constants;

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

        public void Enter() =>
            _sceneLoader.LoadScene(SceneNames.Initial, onLoaded: EnterLoadLevel);

        public void Exit()
        {
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadLevelState, string>(SceneNames.Scene);

        private void RegisterServices()
        {
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IGameObjectFactory>(new GameObjectFactory(_services.Single<IAssetProvider>()));
            RegisterInputService();
        }

        private void RegisterInputService() => //TODO: for different platforms
            RegisterMobileInputService();


        private void RegisterMobileInputService()
        {
            MobileInputService mobileInputService = _services.Single<IGameObjectFactory>().CreateMobileInputService();
            _services.RegisterSingle<IInputService>(mobileInputService);
            _services.RegisterSingle<IUIInputController>(mobileInputService);
        }
    }
}