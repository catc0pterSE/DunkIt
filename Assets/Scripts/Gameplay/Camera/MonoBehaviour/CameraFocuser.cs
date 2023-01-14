using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Camera.MonoBehaviour
{
    public class CameraFocuser : SwitchableComponent
    {
        [SerializeField] private float _defaultCameraRotationSpeed;
        
        private Transform _lookTarget;
        private Coroutine _changingTarget;
        private bool _isChangingTarget;

        private void Update()
        {
            Focus();
        }

        public void SetTarget(Transform target, bool instantly)
        {
            _lookTarget = target;
            
            if (instantly)
                transform.LookAt(target);
        }

        private void Focus()
        {
            if (_lookTarget == null)
                return;
            
            Vector3 newDirection = _lookTarget.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(newDirection);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, toRotation, _defaultCameraRotationSpeed * Time.deltaTime);
            
        }
    }
}