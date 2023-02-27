using System;
using System.Collections;
using Modules.MonoBehaviour;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Minigame.FightForBall.UI
{
    public class Timer : SwitchableMonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _minigameTime = 5;
        [SerializeField] private int _charactersAfterDot = 2;

        private float _passedTime;
        private Coroutine _countingRoutine;

        public event Action TimeOver;

        private void OnEnable()
        {
            _passedTime = _minigameTime;
            UpdateText();
        }

        private void OnDisable()
        {
            StopCounting();
        }

        public void Launch()
        {
            StopCounting();

            _countingRoutine = StartCoroutine(CountTime());
        }

        private void StopCounting()
        {
            if (_countingRoutine != null)
                StopCoroutine(_countingRoutine);
        }

        private void UpdateText() =>
            _text.text = $"{Math.Round(_passedTime, _charactersAfterDot)}";

        private IEnumerator CountTime()
        {
            while (_passedTime > 0)
            {
                UpdateText();
                _passedTime -= Time.deltaTime;
                yield return null;
            }

            TimeOver?.Invoke();
        }
    }
}