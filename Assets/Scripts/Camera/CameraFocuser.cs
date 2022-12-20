using System;
using UnityEngine;

namespace Camera
{
    public class CameraFocuser : MonoBehaviour
    {
        private Transform _lookTarget;

        private Vector3 LookTarget =>
            _lookTarget.position; //TODO: when Player has ball -> enemy basket, when he don't -  character with ball.

        private void FixedUpdate()
        {
            Focus();
        }

        public void SetTarget(Transform target)
        {
            _lookTarget = target;
        }

        private void Focus()
        {
            transform.LookAt(LookTarget);
        }
    }
}