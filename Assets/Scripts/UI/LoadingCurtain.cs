using System.Collections;
using UnityEngine;

namespace UI
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _curtain;
        
        private const float FadingStep = 0.005f;
        private float _defaultCurtainAlpha;
        private Coroutine _fadeJob;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _defaultCurtainAlpha = _curtain.alpha;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _curtain.alpha = _defaultCurtainAlpha;
        }

        public void Hide()
        {
            if (_fadeJob !=null)
                StopCoroutine(_fadeJob);

            _fadeJob = StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            while (_curtain.alpha>0)
            {
                _curtain.alpha -= FadingStep;
                yield return null;
            }
            
            gameObject.SetActive(false);
        }
    }
}
