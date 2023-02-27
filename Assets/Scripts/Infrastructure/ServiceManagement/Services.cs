namespace Infrastructure.ServiceManagement
{
    public class Services
    {
        private static Services _instance;

        private static Services Container => _instance ??= new Services();

        public void RegisterSingle<TService>(TService implementation) where TService: IService
        {
            Implementation<TService>.ServiceInstance = implementation;
        }
        
        public TService Single<TService>() where TService: IService =>
            Implementation<TService>.ServiceInstance; 
        
        private static class Implementation<TService> where TService : IService //TODO: dictionary
        {
            public static TService ServiceInstance;
        }
    }
}