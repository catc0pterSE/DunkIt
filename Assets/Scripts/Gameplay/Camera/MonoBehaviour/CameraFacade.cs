using Gameplay.Camera.StateMachine;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Camera.MonoBehaviour
{
    public class CameraFacade : UnityEngine.MonoBehaviour
    {
        [SerializeField] private CameraFocuser _focuser;
        [SerializeField] private CameraTargetFollower _targetFollower;

        private CameraStateMachine _stateMachine;
        public CameraStateMachine StateMachine => _stateMachine ??= new CameraStateMachine(this);

        public void SetFollowTarget(Transform target) =>
            _targetFollower.SetTarget(target);

        public void SetFocusTarget(Transform target, bool instantly = true, float changingTargetSpeed = NumericConstants.DefaultCameraChangingTargetSpeed)
        {
          if (instantly)
              _focuser.SetTarget(target);
          else
              _focuser.SetTarget(target, changingTargetSpeed);
        }

        public void EnableTargetFollowing() =>
            _targetFollower.Enable();

        public void DisableTargetFollowing() =>
            _targetFollower.Disable();

        public void EnableFocusing() =>
            _focuser.Enable();

        public void DisableFocusing() =>
            _focuser.Disable();
    }
}