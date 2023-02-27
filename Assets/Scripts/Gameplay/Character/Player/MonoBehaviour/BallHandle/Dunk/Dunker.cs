using System;
using System.Collections;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using Modules.MonoBehaviour;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;
using Random = UnityEngine.Random;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Dunk
{
    using Ball.MonoBehavior;

    public class Dunker : SwitchableComponent
    {
        [SerializeField] private PlayerMover _mover;
        [SerializeField] private float _jumpTime = 1f;
        [SerializeField] private float _timeToDunk = 0.75f;
        [SerializeField] private float _highestPoint = 1;
        [SerializeField] private float _timeToHighestPoint = 0.5f;
        [SerializeField] private float _dunkThrowForce = 5;

        private AnimationCurve _xPositionCurve;
        private AnimationCurve _yPositionCurve;
        private AnimationCurve _zPositionCurve;
        private Ball _ball;
        private Coroutine _dunking;

        public event Action DunkPointReached;

        public void Initialize(Ball ball)
        {
            _ball = ball;
        }

        public void Dunk(Ring ring)
        {
            if (_dunking != null)
                StopCoroutine(_dunking);

            Vector3 startPosition = transform.position;
            Vector3 targetPosition = ring.DunkPoints.GetTransformPositions().FindClosest(startPosition);
            SetUpCurves(startPosition, targetPosition);
            _dunking = StartCoroutine(DunkRoutine(ring.BallDunkPoint.position));
        }

        private void SetUpCurves(Vector3 startPosition, Vector3 targetPosition)
        {
            SetXPositionCurve(startPosition, targetPosition);
            SetYPositionCurve(startPosition, targetPosition);
            SetZPositionCurve(startPosition, targetPosition);
        }

        private void SetXPositionCurve(Vector3 startPosition, Vector3 targetPosition)
        {
            _xPositionCurve = new AnimationCurve();
            _xPositionCurve.AddKey(new Keyframe(0, startPosition.x));
            _xPositionCurve.AddKey(_timeToDunk, targetPosition.x);
            _xPositionCurve.AddKey(_jumpTime, targetPosition.x);
        }

        private void SetYPositionCurve(Vector3 startPosition, Vector3 targetPosition)
        {
            _yPositionCurve = new AnimationCurve();
            _yPositionCurve.AddKey(new Keyframe(0, startPosition.y));
            _yPositionCurve.AddKey(new Keyframe(_timeToHighestPoint, targetPosition.y + _highestPoint));
            _yPositionCurve.AddKey(_timeToDunk, targetPosition.y);
        }

        private void SetZPositionCurve(Vector3 startPosition, Vector3 targetPosition)
        {
            _zPositionCurve = new AnimationCurve();
            _zPositionCurve.AddKey(new Keyframe(0, startPosition.z));
            _zPositionCurve.AddKey(_timeToDunk, targetPosition.z);
            _yPositionCurve.AddKey(_jumpTime, startPosition.y);
            _zPositionCurve.AddKey(_jumpTime, targetPosition.z);
        }

        private IEnumerator DunkRoutine(Vector3 ballThrowPoint)
        {
            float time = 0;
            bool isBallThrown = false;

            while (time < _jumpTime)
            {
                transform.position = new Vector3(_xPositionCurve.Evaluate(time), _yPositionCurve.Evaluate(time),
                    _zPositionCurve.Evaluate(time));

                _mover.RotateTo(ballThrowPoint);

                if (time > _timeToDunk && isBallThrown == false)
                {
                    isBallThrown = true;
                    DunkPointReached?.Invoke();
                    ThrowBall(ballThrowPoint);
                }

                time += Time.deltaTime;
                yield return null;
            }
        }

        private void ThrowBall(Vector3 ballThrowPoint)
        {
            _ball.MoveTo(ballThrowPoint, () =>
            {
                Vector3 throwDirection =
                    new Vector3
                    (
                        Vector3.down.x + Random.Range(0, NumericConstants.Half),
                        Vector3.down.y,
                        Vector3.down.z + Random.Range(0, NumericConstants.Half)
                    ) * _dunkThrowForce;
                _ball.Throw(throwDirection);
            });
        }
    }
}