using DG.Tweening;
using Gameplay.Minigame.FightForBall.UI;
using Modules.MonoBehaviour;
using UnityEngine;
using Random = UnityEngine.Random;

namespace z_Test
{
    public class FightForBallUI_dotweenBezier : SwitchableMonoBehaviour
    {
        [SerializeField] private Ball _ball;
        [SerializeField] private float _maxSegmentDistance = 220;
        [SerializeField] private float _minSegmentDistance = 180;
        [SerializeField] private float _minigameTime = 6;
        [SerializeField] private int _pointsNumber = 10;

        private readonly WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

        private Vector3[] _path;

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
            InitializePath();
            _ball.transform.DOPath(_path, _minigameTime, PathType.CubicBezier, PathMode.Sidescroller2D);
        }

        private void InitializePath()
        {
            _path = new Vector3[_pointsNumber * 3];

            for (int i = 0; i < _path.Length; i++)
            {
                _path[i] = i == 0 /*|| i == _path.Length - 3*/
                    ? _ball.transform.position
                    : i > 0 && i % 3 == 0
                        ? CalculateTargetPosition(_path[i - 3])
                        : (i + 1) % 3 == 0
                            ? GetNormal(i)
                            : GetRandomVector();
            }
        }


        private Vector3 GetNormal(int index)
        {
            Vector3 vector1 = _path[index - 2];
            Vector3 vector2 = _path[index - 1];


            Vector3 direction = vector2 - vector1;

            return vector2 - direction;
        }

        private Vector3 CalculateTargetPosition(Vector3 startPosition)
        {
            Vector3 direction = Random.insideUnitCircle.normalized;
            Vector3 roughPosition = startPosition + direction * Random.Range(_minSegmentDistance, _maxSegmentDistance);
            Vector3 clampedPosition = new Vector3
            (
              //  Mathf.Clamp(roughPosition.x, _ball.Offset, Screen.width - _ball.Offset),
               // Mathf.Clamp(roughPosition.y, _ball.Offset, Screen.height - _ball.Offset),
               // 0
            );

            return clampedPosition;

            /*Vector3 point = GetRandomVector();
            Vector3 toPoint = point - startPosition;

            return point;*/

            /*Vector3 direction = Random.insideUnitCircle.normalized;
            Vector3 point = startPosition + direction * Random.Range(_minSegmentDistance, _maxSegmentDistance);

            if (point.x < _ball.Offset || point.x > Screen.width - _ball.Offset ||
                point.y < _ball.Offset || point.y > Screen.height * NumericConstants.Half)
                point = CalculateTargetPosition(startPosition);

            return point;*/
        }

        private Vector3 GetRandomVector() =>
            new Vector3
            (
              //  Random.Range(_ball.Offset, Screen.width - _ball.Offset),
               // Random.Range(_ball.Offset, Screen.height * NumericConstants.Half),
               // 0
            );
    }
}