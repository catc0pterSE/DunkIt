using Infrastructure.Provider;
using UnityEngine;
using Utility.Static;

namespace Infrastructure.Factory
{
    public class GameObjectFactory : IGameObjectFactory
    {
        private readonly IAssetProvider _assetProvider;

        public GameObjectFactory(IAssetProvider assetProvider)
        {
           _assetProvider = assetProvider;
        }
        
        public GameObject CreateLoadingCurtain()
        {
            return _assetProvider.Instantiate(ResourcesPathes.LoadingCurtainPath);
        }
        
        public GameObject CreatePlayer(Vector3 at)
        {
            
             return _assetProvider.Instantiate(ResourcesPathes.PlayerPath, at);
        }
        
        public GameObject CreateHUD()
        {
            return _assetProvider.Instantiate(ResourcesPathes.HUDPath);
        }

        public GameObject CreateCamera()
        {
            return _assetProvider.Instantiate(ResourcesPathes.CameraPath);
        }

    }
}