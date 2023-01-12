using System.Collections;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Camera.MonoBehaviour
{
    public class CameraFocuser : SwitchableComponent
    {
        [SerializeField] private float _rotationSpeed = 400;
        
        private Transform _lookTarget;
        private Coroutine _changingTarget;
        
        private void Update()
        {
            Focus();
        }

        public void SetTarget(Transform target, bool smoothly = false)
        {
            if (_lookTarget == target)
                return;
            
            if (smoothly == false)
            {
                _lookTarget = target;
            }
            else
            {
                if (_changingTarget!=null)
                    StopCoroutine(_changingTarget);

                _changingTarget = StartCoroutine(ChangeTarget(target));
            }
        }

        private void Focus()
        {
            if (_lookTarget != null)
                transform.LookAt(_lookTarget);
        }

        private IEnumerator ChangeTarget(Transform target)
        {
            _lookTarget = null;
            Vector3 newDirection = target.position - transform.position;

            while (Vector3.Angle(transform.forward, newDirection)>NumericConstants.MinimalDelta)
            {
                Quaternion toRotation = Quaternion.LookRotation(newDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
                yield return null;
            }

            _lookTarget = target;
        }
    }
}