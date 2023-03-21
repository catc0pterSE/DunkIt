using System;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.Minigame.FightForBall.UI
{
    public class FightForBallUI : SwitchableMonoBehaviour
    {
        [SerializeField] private float _defaultLesserScreenSize = 500;
        [SerializeField] private Ball _ball;
        [SerializeField] private Hand[] _hands;
        [SerializeField] private SafeZone _safeZone;
        [SerializeField] private Timer _timer;
        [SerializeField] private float _defaultHandPerpendicularOffset;
        [SerializeField] private float _defaultHandParallelOffset;
        [SerializeField] private float _defaultParallelDistanceBetweenHands;
        [SerializeField] private float _defaultMinimalDistanceBetweenHandAndBall;

        private float ScreenRatio => (float)Screen.height / Screen.width;
        private Hand LeftHand => _hands.FindFirstOrNull(hand => hand.IsMirrored);
        private Hand RightHand => _hands.FindFirstOrNull(hand => hand.IsMirrored == false);
        private Hand BottomHand => _hands.FindFirstOrNull(hand => hand.IsMirrored == false);
        private Hand TopHand => _hands.FindFirstOrNull(hand => hand.IsMirrored);
        private float ScaledHandPerpendicularOffset => _defaultHandPerpendicularOffset * ScaleModifier;
        private float ScaledHandParallelOffset => _defaultHandParallelOffset * ScaleModifier;
        private float ScaledParallelDistanceBetweenHands => _defaultParallelDistanceBetweenHands * ScaleModifier;

        private float ScaledMinimalDistanceBetweenHandAndBall =>
            _defaultMinimalDistanceBetweenHandAndBall * ScaleModifier;

        public float ScaleModifier => IsScreenVertical
            ? Screen.width / _defaultLesserScreenSize
            : Screen.height / _defaultLesserScreenSize;

        public Vector3 Scale => new Vector3(ScaleModifier, ScaleModifier, ScaleModifier);

        public bool IsScreenVertical => ScreenRatio > 1;

        public event Action Won;
        public event Action Lost;

        private void OnEnable()
        {
            LocateContents();
            _ball.Touched += StartMinigame;
        }

        private void OnDisable()
        {
            _ball.Touched -= StartMinigame;
            _timer.TimeOver -= FinishSuccessful;
            _hands.Map(hand => hand.BallCaught -= FinishUnsuccessful);
            _safeZone.BallReached -= FinishSuccessful;
        }

        private void StartMinigame()
        {
            _ball.Touched -= StartMinigame;
            _timer.Launch();
            _timer.TimeOver += FinishSuccessful;
            _safeZone.BallReached += FinishSuccessful;
            _hands.Map(hand =>
            {
                hand.BallCaught += FinishUnsuccessful;
                hand.Launch();
            });
        }

        private void FinishSuccessful()
        {
            _hands.Map(hand => hand.Stop());
            Won?.Invoke();
        }
           

        private void FinishUnsuccessful() =>
            Lost?.Invoke();

        private void LocateContents()
        {
            if (IsScreenVertical)
                LocateVertical();
            else
                LocateHorizontal();

            DisableHandsTooClose();
        }

        private void LocateVertical()
        {
            _ball.transform.position =
                new Vector3(Screen.width * NumericConstants.Half, _ball.ScaledOffset);
            _safeZone.transform.position =
                new Vector3(Screen.width * NumericConstants.Half, Screen.height - _safeZone.ScaledOffset);

            float upper = Screen.height - ScaledHandParallelOffset;
            float lower = upper - ScaledParallelDistanceBetweenHands;
            float left = Screen.width * NumericConstants.Half - ScaledHandPerpendicularOffset;
            float right = Screen.width * NumericConstants.Half + ScaledHandPerpendicularOffset;

            Vector3[] possibleRightPositions = new Vector3[]
            {
                new Vector3(right, upper),
                new Vector3(right, lower)
            };

            Vector3[] possibleLeftPositions = new Vector3[]
            {
                new Vector3(left, upper),
                new Vector3(left, lower)
            };
            Vector3 rightHandPosition = RightHand.transform.position = possibleRightPositions.GetRandom();
            LeftHand.transform.position = possibleLeftPositions.FindFarthest(rightHandPosition);
        }

        private void LocateHorizontal()
        {
            _ball.transform.position =
                new Vector3(_ball.ScaledOffset, Screen.height * NumericConstants.Half);
            _safeZone.transform.position =
                new Vector3(Screen.width - _safeZone.ScaledOffset, Screen.height * NumericConstants.Half);

            float top = Screen.height * NumericConstants.Half + ScaledHandPerpendicularOffset;
            float bottom = Screen.height * NumericConstants.Half - ScaledHandPerpendicularOffset;
            float right = Screen.width - ScaledHandParallelOffset;
            float left = right - ScaledParallelDistanceBetweenHands;

            Vector3[] possibleTopPositions = new Vector3[]
            {
                new Vector3(left, top),
                new Vector3(right, top)
            };

            Vector3[] possibleBottomPositions = new Vector3[]
            {
                new Vector3(left, bottom),
                new Vector3(right, bottom)
            };

            Vector3 topHandPosition = TopHand.transform.position = possibleTopPositions.GetRandom();
            BottomHand.transform.position = possibleBottomPositions.FindFarthest(topHandPosition);
        }

        private void DisableHandsTooClose()
        {
            foreach (Hand hand in _hands)
            {
                if (Vector3.Distance(hand.transform.position, _ball.transform.position) <
                    ScaledMinimalDistanceBetweenHandAndBall)
                    hand.Disable();
            }
        }
    }
}