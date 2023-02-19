using System;
using System.Linq;
using Gameplay.Minigame.FightForBall.UI.Hands;
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
        [SerializeField] private Hand _rightHand;
        [SerializeField] private Hand _leftHand;
        [SerializeField] private SafeZone _safeZone;
        [SerializeField] private Timer _timer;
        [SerializeField] private Transform[] _leftHandStartPositions;
        [SerializeField] private Transform[] _rightHandStartPositions;
        [SerializeField] private Transform[] _topHandStartPositions;
        [SerializeField] private Transform[] _bottomHandStartPositions;

        public float ScreenRatio => (float)Screen.height / Screen.width;

        public float ScaleModifier => IsScreenVertical
            ? Screen.width / _defaultLesserScreenSize
            : Screen.height / _defaultLesserScreenSize;

        public event Action Won;
        public event Action Lost;

        public Vector3 Scale => new Vector3(ScaleModifier, ScaleModifier, ScaleModifier);

        public bool IsScreenVertical => ScreenRatio > 1;

        private void OnEnable()
        {
            LocateContents();
            _ball.Touched += StartMinigame;
        }

        private void OnDisable()
        {
            _ball.Touched -= StartMinigame;
            _timer.TimeOver -= FinishSuccessful;
            _leftHand.BallCaught -= FinishUnsuccessful;
            _rightHand.BallCaught -= FinishUnsuccessful;
            _safeZone.BallReached -= FinishSuccessful;
        }

        private void StartMinigame()
        {
            _ball.Touched -= StartMinigame;
            _timer.Launch();
            _timer.TimeOver += FinishSuccessful;
            _leftHand.BallCaught += FinishUnsuccessful;
            _rightHand.BallCaught += FinishUnsuccessful;
            _safeZone.BallReached += FinishSuccessful;
            _leftHand.Launch();
            _rightHand.Launch();
        }

        private void FinishSuccessful()
        {
            Lost?.Invoke();
            Debug.Log("Won");
            Time.timeScale = 0;
        }

        private void FinishUnsuccessful()
        {
            Won?.Invoke();
            Time.timeScale = 0;
            Debug.Log("Lost");
        }


        private void DisableHands()
        {
            _rightHand.Disable();
            _leftHand.Disable();
        }

        private void LocateContents()
        {
            if (IsScreenVertical)
            {
                _ball.transform.position =
                    new Vector3(Screen.width * NumericConstants.Half, _ball.ScaledOffset);
                _safeZone.transform.position =
                    new Vector3(Screen.width * NumericConstants.Half, Screen.height - _safeZone.ScaledOffset);

                Vector3[] leftPositions = _leftHandStartPositions.GetTransformPositions();
                Vector3 randomLeftPosition = leftPositions.GetRandom();
                _leftHand.transform.position = randomLeftPosition;
                Vector3[] rightPositions = _rightHandStartPositions.GetTransformPositions();
                Vector3[] filteredRightPositions = rightPositions
                    .Where(position => Math.Abs(position.y - randomLeftPosition.y) > NumericConstants.MinimalDelta)
                    .ToArray();
                Vector3 randomRightPosition = filteredRightPositions.GetRandom();
                _rightHand.transform.position = randomRightPosition;
            }
            else
            {
                _ball.transform.position =
                    new Vector3(_ball.ScaledOffset, Screen.height * NumericConstants.Half);
                _safeZone.transform.position =
                    new Vector3(Screen.width - _safeZone.ScaledOffset, Screen.height * NumericConstants.Half);

                Vector3[] topPositions = _topHandStartPositions.GetTransformPositions();
                Vector3 randomTopPosition = topPositions.GetRandom();
                _leftHand.transform.position = randomTopPosition;
                Vector3[] bottomPositions = _bottomHandStartPositions.GetTransformPositions();
                Vector3[] filteredBottomPositions = bottomPositions
                    .Where(position => Math.Abs(position.x - randomTopPosition.x) > NumericConstants.MinimalDelta)
                    .ToArray();
                Vector3 randomBottomPosition = filteredBottomPositions.GetRandom();
                _rightHand.transform.position = randomBottomPosition;
            }
        }
    }
}