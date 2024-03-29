﻿using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    using await = Cysharp.Threading.Tasks;
    public class SceneLoader
    {
        public async void LoadScene(string name, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                return;
            }

            await SceneManager.LoadSceneAsync(name); //TODO: cancellation? progressbar?

            onLoaded?.Invoke();
        }
    }
}