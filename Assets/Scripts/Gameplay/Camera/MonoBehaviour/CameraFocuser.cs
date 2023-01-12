using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Camera.MonoBehaviour
{
    public class CameraFocuser : SwitchableComponent
    {
        private Transform _lookTarget;

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
            transform.LookAt(_lookTarget);
        }
    }
}