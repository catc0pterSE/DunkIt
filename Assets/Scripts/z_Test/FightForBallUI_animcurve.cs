using System.Collections;
using Gameplay.Minigame.FightForBall.UI;
using Modules.MonoBehaviour;
using UnityEngine;

namespace z_Test
{
    using Random = UnityEngine.Random;

namespace Gameplay.Minigame.FightForBall.UI
{
    public class FightForBallUI_animcurve : SwitchableMonoBehaviour
    {
        [SerializeField] private Ball _ball;
        [SerializeField] private float _maxSegmentDistance = 220;
        [SerializeField] private float _minSegmentDistance = 180;
        [SerializeField] private float _minigameTime = 6;
        [SerializeField] private int _segmentsNumber = 10;

        private readonly WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
        
        [SerializeField] private AnimationCurve _positionXCurve;
        [SerializeField] private AnimationCurve _positionYCurve;
        private Coroutine _movingRoutine;

        private void OnEnable()
        {
            _ball.Touched += Launch;
        }

        private void OnDisable()
        {
            _ball.Touched -= Launch;
        }

        private void Launch()
        {
            InitializeCurves();

            if (_movingRoutine != null)
                StopCoroutine(_movingRoutine);

            _movingRoutine = StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            float time = 0;

            while (time < _minigameTime)
            {
                _ball.transform.position = new Vector3
                (
                    _positionXCurve.Evaluate(time),
                    _positionYCurve.Evaluate(time),
                    0
                );

                time += Time.deltaTime;
                yield return null;
            }
        }

        private void InitializeCurves()
        {
            _positionXCurve = new AnimationCurve();
            _positionYCurve = new AnimationCurve();
            Vector3[] points = new Vector3[_segmentsNumber+1];
            float timePerSegment = _minigameTime / (_segmentsNumber);

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = i == 0 || i== points.Length-1
                    ? _ball.transform.position
                    : CalculateTargetPosition(points[i - 1]);

                float pointTiming = timePerSegment * i;
                Keyframe keyframeX = new Keyframe(pointTiming, points[i].x);
                Keyframe keyframeY = new Keyframe(pointTiming, points[i].y);
                _positionXCurve.AddKey(keyframeX);
                _positionYCurve.AddKey(keyframeY);
                _positionXCurve.SmoothTangents(i, 1);
                _positionYCurve.SmoothTangents(i, 1);
            }
        }

        private Vector3 CalculateTargetPosition(Vector3 startPosition)
        {
            Vector3 direction = Random.insideUnitCircle.normalized;
            Vector3 roughPosition = startPosition + direction * Random.Range(_minSegmentDistance, _maxSegmentDistance);
            Vector3 clampedPosition = new Vector3
            (
               // Mathf.Clamp(roughPosition.x, _ball.Offset, Screen.width - _ball.Offset),
               // Mathf.Clamp(roughPosition.y, _ball.Offset, Screen.height*NumericConstants.Half),
              //  0
            );
            
            return clampedPosition;

            /*Vector3 point = GetRandomVector();
            Vector3 toPoint = point - startPosition;

            return point;*/

            /*
            Vector3 direction = Random.insideUnitCircle.normalized;
            Vector3 point = startPosition + direction * Random.Range(_minSegmentDistance, _maxSegmentDistance);

            if (point.x < _ball.Offset || point.x > Screen.width - _ball.Offset ||
                point.y < _ball.Offset || point.y > Screen.height - _ball.Offset)
                point = CalculateTargetPosition(startPosition);

            return point;*/
        }

        /*private Vector3 GetRandomVector()=>
            new Vector3
            (
                Random.Range(_ball.Offset, Screen.width - _ball.Offset),
                Random.Range(_ball.Offset, Screen.height - _ball.Offset),
                0
            );*/
    }
}
}