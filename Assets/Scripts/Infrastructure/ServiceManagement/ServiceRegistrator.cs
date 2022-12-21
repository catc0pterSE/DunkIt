using Infrastructure.Factory;
using Infrastructure.Input;
using Infrastructure.Provider;

namespace Infrastructure.ServiceManagement
{
    public class ServiceRegistrator
    {
        public void RegisterServices()
        {
            Services.Container.RegisterSingle<IInputService>(new SimpleInputService()); //TODO: for different platforms
            Services.Container.RegisterSingle<IAssetProvider>(new AssetProvider());
            Services.Container.RegisterSingle<IGameObjectFactory>(new GameObjectFactory(Services.Container.Single<IAssetProvider>()));
        }
    }
}