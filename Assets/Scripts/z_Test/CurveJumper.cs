using System;
using System.Collections;
using Scene.Ring;
using UnityEngine;
using Utility.Extensions;

namespace z_Test
{
    public class CurveJumper : MonoBehaviour
    {
        [SerializeField] private Ring _ring;
        [SerializeField] private float _jumpTime = 1f;
        [SerializeField] private float _timeToDunk = 0.75f;
        [SerializeField] private float _highestPoint = 1;
        [SerializeField] private float _timeToHighestPoint = 0.5f;

        private AnimationCurve _xPosition;
        private AnimationCurve _yPosition;
        private AnimationCurve _zPosition;
        

        private Coroutine _jumping;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ALLLA();
            }
        }

        private void ALLLA()
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = _ring.DunkPoints.GetTransformPositions().FindClosest(startPosition);
            
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

            if (_jumping != null)
                StopCoroutine(_jumping);

            _jumping = StartCoroutine(Jump());
        }

        private IEnumerator Jump()
        {
            float time = 0;

            while (time < _jumpTime)
            {
                transform.position = new Vector3(_xPosition.Evaluate(time), _yPosition.Evaluate(time),
                    _zPosition.Evaluate(time));

                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}