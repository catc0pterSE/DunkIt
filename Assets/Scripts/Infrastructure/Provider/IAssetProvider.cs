using Infrastructure.ServiceManagement;
using UnityEngine;

namespace Infrastructure.Provider
{
    public interface IAssetProvider : IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 point);
    }
}