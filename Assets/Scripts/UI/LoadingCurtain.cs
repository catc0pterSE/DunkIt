using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _curtain;
        
        private const float FadingStep = 0.03f;

        private float _defaultCurtainAlpha;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _defaultCurtainAlpha = _curtain.alpha;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public async void Hide()
        {
            while (_curtain.alpha>0)
            {
                _curtain.alpha -= FadingStep;
                await UniTask.NextFrame();
            }
            
            gameObject.SetActive(false);
        }
        
    }
}
