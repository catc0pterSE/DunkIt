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

        private AnimationCurve _xPosition;
        private AnimationCurve _yPosition;
        private AnimationCurve _zPosition;
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
            Vector3 targetPosition = ring.DunkPoints.FindClosest(startPosition).position;
            SetUpCurves(startPosition, targetPosition);

            _dunking = StartCoroutine(DunkRoutine(ring.BallDunkPoint.position));
        }

        private void SetUpCurves(Vector3 startPosition, Vector3 targetPosition)
        {
            _xPosition = new AnimationCurve();
            _yPosition = new AnimationCurve();
            _zPosition = new AnimationCurve();
            _xPosition.AddKey(new Keyframe(0, startPosition.x));
            _yPosition.AddKey(new Keyframe(0, startPosition.y));
            _zPosition.AddKey(new Keyframe(0, startPosition.z));
            _yPosition.AddKey(new Keyframe(_timeToHighestPoint, targetPosition.y + _highestPoint));
            _xPosition.AddKey(_timeToDunk, targetPosition.x);
            _yPosition.AddKey(_timeToDunk, targetPosition.y);
            _zPosition.AddKey(_timeToDunk, targetPosition.z);
            _xPosition.AddKey(_jumpTime, targetPosition.x);
            _yPosition.AddKey(_jumpTime, startPosition.y);
            _zPosition.AddKey(_jumpTime, targetPosition.z);
        }

        private IEnumerator DunkRoutine(Vector3 ballThrowPoint)
        {
            float time = 0;
            bool isBallThrown = false;

            while (time < _jumpTime)
            {
                transform.position = new Vector3(_xPosition.Evaluate(time), _yPosition.Evaluate(time),
                    _zPosition.Evaluate(time));

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
            _ball.RemoveOwner();
            _ball.transform.position = ballThrowPoint;
            Vector3 throwDirection =
                new Vector3
                (
                    Vector3.down.x + Random.Range(0, NumericConstants.Half),    
                    Vector3.down.y,
                    Vector3.down.z + Random.Range(0, NumericConstants.Half)
                ) *
                NumericConstants.DunkThrowForce;
            
            _ball.Throw(throwDirection);
        }
    }
}