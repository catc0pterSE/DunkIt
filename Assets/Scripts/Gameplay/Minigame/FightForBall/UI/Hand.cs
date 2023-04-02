using System;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Minigame.FightForBall.UI
{
    public class Hand : SwitchableMonoBehaviour //TODO: not run after ball but just slide?
    {
        [SerializeField] private FightForBallUI _fightForBallUI;
        [SerializeField] private Ball _ball;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private bool _isMirrored;
        [SerializeField] private float _defaultMovementSpeed;
        [SerializeField] private float _offScreenPosition;

        private bool _isLaunched;
        private float ScaledMaxLength => Math.Abs(_endPoint.localPosition.x) * ScaleModifier;
        private float ScaledOffScreenPosition => _offScreenPosition * ScaleModifier;
        private float ScaledMovementSpeed => _defaultMovementSpeed * ScaleModifier;
        private float ScaleModifier => _fightForBallUI.ScaleModifier;
        public bool IsMirrored => _isMirrored;

        public event Action BallCaught;

        private void OnEnable()
        {
            _isLaunched = false;
            transform.localScale = _fightForBallUI.Scale;
            SetRotation();
        }

        private void OnDisable()
        {
            transform.localScale = Vector3.one;
        }

        private void Update()
        {
            if (_isLaunched == false)
                return;

            if (_fightForBallUI.IsScreenVertical)
                SetVerticalPosition();
            else
                SetHorizontalPosition();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Ball ball))
            {
                ball.Stop();
                BallCaught?.Invoke();
            }
        }

        public void Launch() =>
            _isLaunched = true;

        public void Stop() =>
            _isLaunched = false;


        private Vector3 GetPositionToBall() =>
            Vector3.MoveTowards
            (
                transform.position,
                _ball.transform.position,
                Time.deltaTime * ScaledMovementSpeed
            );

        private void SetVerticalPosition()
        {
            Vector3 newPosition = GetPositionToBall();

            float clampedX = _isMirrored
                ? Mathf.Clamp(newPosition.x, -ScaledOffScreenPosition, ScaledMaxLength)
                : Mathf.Clamp(newPosition.x, Screen.width - ScaledMaxLength, Screen.width + ScaledOffScreenPosition);

            transform.position = new Vector3(clampedX, newPosition.y);
        }

        private void SetHorizontalPosition()
        {
            Vector3 newPosition = GetPositionToBall();

            float clampedY = _isMirrored
                ? Mathf.Clamp(newPosition.y, Screen.height - ScaledMaxLength, Screen.height + ScaledOffScreenPosition)
                : Mathf.Clamp(newPosition.y, -ScaledOffScreenPosition, ScaledMaxLength);

            transform.position = new Vector3(newPosition.x, clampedY);
        }

        private void SetRotation()
        {
            Vector3 newEulerAngles;

            if (_fightForBallUI.IsScreenVertical)
            {
                newEulerAngles = _isMirrored
                    ? new Vector3(0, NumericConstants.FlatAngle, 0)
                    : Vector3.zero;
            }
            else
            {
                newEulerAngles = _isMirrored
                    ? new Vector3(0, 180, NumericConstants.RightAngle)
                    : new Vector3(0, 0, -NumericConstants.RightAngle);
            }

            transform.eulerAngles = newEulerAngles;
        }
    }
}