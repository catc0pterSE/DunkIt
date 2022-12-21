using Infrastructure.ServiceManagement;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameObjectFactory : IService
    {
        public GameObject CreatePlayer(Vector3 at);
        public GameObject CreateHUD();
        public GameObject CreateCamera();

        public GameObject CreateLoadingCurtain();
    }
}