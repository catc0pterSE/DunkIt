using System;
using System.Collections;
using Infrastructure.Input;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Minigame.JumpBall
{
    public class JumpBallUI: SwitchableMonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private float _speed;
        [SerializeField] private MinigameHandle _handle;

        private IInputService _inputService;
        private Coroutine _running;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();

        private void OnEnable()
        {
            Reset();
            Launch();
        }

        public event Action Won;
        public event Action Lost;

        private void Launch()
        {
            Enable();

            if (_running != null)
                StopCoroutine(_running);

            StartCoroutine(Run());
        }

        private void Start()
        {
            Launch();
        }

        private IEnumerator Run()
        {
            SubscribeOnTouch();
            while (_slider.value < _slider.maxValue)
            {
                _slider.value = Mathf.MoveTowards(_slider.value, _slider.maxValue, _speed * Time.deltaTime);

                yield return null;
            }

            Finish();
        }

        private void SubscribeOnTouch()
        {
            InputService.TouchDown += Finish;
        }
        
        private void UnsubscribeFromTouch()
        {
            InputService.TouchDown -= Finish;
        }
        

        private void Reset() =>
            _slider.value = _slider.minValue;

        private void Finish()
        {
            UnsubscribeFromTouch();
            
            if (_handle.IsInZone)
                Won?.Invoke();
            else
                Lost?.Invoke();
        }  
    }
}