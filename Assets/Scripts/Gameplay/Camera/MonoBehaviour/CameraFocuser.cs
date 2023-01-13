using System.Collections;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Camera.MonoBehaviour
{
    public class CameraFocuser : SwitchableComponent
    {
        private Transform _lookTarget;
        private Coroutine _changingTarget;
        private bool _isChangingTarget;

        private void Update()
        {
            Focus();
        }

        public void SetTarget(Transform target)
        {
            _lookTarget = target;
        }

        public void SetTarget(Transform target, float changingTargetSpeed)
        {
            if (_changingTarget != null)
                StopCoroutine(_changingTarget);

            _changingTarget = StartCoroutine(ChangeTarget(target, changingTargetSpeed));
        }


        private void Focus()
        {
            if (_isChangingTarget == false)
                transform.LookAt(_lookTarget);
        }

        private IEnumerator ChangeTarget(Transform target, float changingTargetSpeed)
        {
            _isChangingTarget = true;
            Vector3 newDirection = target.position - transform.position;

            while (Vector3.Angle(transform.forward, newDirection) > NumericConstants.MinimalDelta)
            {
                Quaternion toRotation = Quaternion.LookRotation(newDirection, Vector3.up);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, toRotation, changingTargetSpeed * Time.deltaTime);
                yield return null;
            }

            _lookTarget = target;
            _isChangingTarget = false;
        }
    }
}