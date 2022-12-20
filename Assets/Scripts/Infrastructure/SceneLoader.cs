using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader
    {
        public async void LoadScene(string name, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                return;
            }

            AsyncOperation waitSceneLoad = SceneManager.LoadSceneAsync(name);

            while (waitSceneLoad.isDone == false)
            {
                await UniTask.NextFrame();
            }

            onLoaded?.Invoke();
        }
    }
}