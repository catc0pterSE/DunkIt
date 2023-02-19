using System;
using System.Linq;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Minigame.FightForBall.UI
{
    public class SafeZone : SwitchableMonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private FightForBallUI _fightForBallUI;
        [SerializeField] private AnimationCurve _scaleCurve;

        private float _passedTime;
        private Vector3 Scale => _fightForBallUI.Scale;
        private float _animationCycleTime;
        public float ScaledOffset => _rectTransform.rect.height * NumericConstants.Half * ScaleModifier*_scaleCurve.keys.Max(keyframe => keyframe.value);
        private float ScaleModifier => _fightForBallUI.ScaleModifier;

        public event Action BallReached;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Ball>(out _))
                BallReached?.Invoke();
        }

        private void OnEnable()
        {
            transform.localScale = Scale;
            _passedTime = 0;
            _animationCycleTime = _scaleCurve.keys.Last().time;
        }
        
        private void Update()
        {
            if (_passedTime >= _animationCycleTime)
                _passedTime = 0;

            transform.localScale = Scale * _scaleCurve.Evaluate(_passedTime);
            _passedTime += Time.deltaTime;
        }
    }
}