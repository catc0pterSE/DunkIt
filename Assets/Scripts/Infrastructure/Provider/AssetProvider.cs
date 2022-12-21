using UnityEngine;

namespace Infrastructure.Provider
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return GameObject.Instantiate(prefab);
        }

        public GameObject Instantiate(string path, Vector3 point)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return GameObject.Instantiate(prefab, point, Quaternion.identity);
        }
    }
}