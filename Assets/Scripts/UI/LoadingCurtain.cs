using System;
using System.Collections;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;

namespace UI
{
    public class LoadingCurtain : SwitchableMonoBehaviour
    {
        [SerializeField] private CanvasGroup _curtain;
        [SerializeField] private GameObject _text;
        [SerializeField] private float _fadingStep = 0.01f;
        [SerializeField] private float _delay = 0.01f;

        private WaitForSeconds _waitForDelay;

        private float _defaultCurtainAlpha;
        private Coroutine _fadeJob;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _waitForDelay = new WaitForSeconds(_delay);
            _defaultCurtainAlpha = _curtain.alpha;
        }

        private void OnEnable()
        {
            _text.SetActive(true);
        }

        public void Show()
        {
            Enable();
            _curtain.alpha = _defaultCurtainAlpha;
        }

        public void FadeInFadeOut(Action toDoWhenFaded = null, bool withText = false)
        {
            Enable();

            if (withText == false)
                _text.SetActive(false);

            toDoWhenFaded += FadeOut;
            FadeIn(toDoWhenFaded);
        }

        public void FadeIn(Action toDoAfter=null)
        {
            Enable();

            if (_fadeJob != null)
                StopCoroutine(_fadeJob);

            _fadeJob = StartCoroutine(Fade(0, _defaultCurtainAlpha, toDoAfter));
        }

        public void FadeOut()
        {
            Enable();

            if (_fadeJob != null)
                StopCoroutine(_fadeJob);

            _fadeJob = StartCoroutine(Fade(_defaultCurtainAlpha, 0, Disable));
        }

        private IEnumerator Fade(float startAlpha, float targetAlpha, Action toDoWhenFaded = null)
        {
            yield return _waitForDelay; //TODO: THIS IS COSTYL
                
            _curtain.alpha = startAlpha;

            while (Math.Abs(_curtain.alpha - targetAlpha) > Mathf.Epsilon)
            {
                _curtain.alpha = Mathf.MoveTowards(_curtain.alpha, targetAlpha, _fadingStep);
                yield return null;
            }

            toDoWhenFaded?.Invoke();
        }
    }
}