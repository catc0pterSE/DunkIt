using System;
using System.Collections;
using Infrastructure.Input;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace Gameplay.Minigame
{
    public class JumpBall : SwitchableMonoBehaviour, IMinigame
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
        }

        public event Action Wined;
        public event Action Lost;

        public void Launch()
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
            while (_slider.value < _slider.maxValue)
            {
                _slider.value = Mathf.MoveTowards(_slider.value, _slider.maxValue, _speed * Time.deltaTime);

                if (InputService.Clicked)
                    Finish();

                yield return null;
            }

            Finish();
        }

        private void Reset() =>
            _slider.value = _slider.minValue;

        private void Finish()
        {
            if (_handle.IsInZone)
                Wined?.Invoke();
            else
                Lost?.Invoke();
            
            Disable();
        }
    }
}