using Gameplay.Camera.StateMachine;
using UnityEngine;

namespace Gameplay.Camera.MonoBehaviour
{
    public class CameraFacade : UnityEngine.MonoBehaviour
    {
        [SerializeField] private CameraFocuser _focuser;
        [SerializeField] private CameraTargetFollower _targetFollower;
        [SerializeField] private RouteFollower _routeFollower;

        private CameraStateMachine _stateMachine;

        public RouteFollower RouteFollower => _routeFollower;

        public CameraStateMachine StateMachine => _stateMachine ??= new CameraStateMachine(this);

        public void SetFollowTarget(Transform target) =>
            _targetFollower.SetTarget(target);

        public void SetFocusTarget(Transform target) =>
            _focuser.SetTarget(target);

        public void EnableTargetFollowing() =>
            _targetFollower.Enable();

        public void DisableTargetFollowing() =>
            _targetFollower.Disable();

        public void EnableFocusing() =>
            _focuser.Enable();

        public void DisableFocusing() =>
            _focuser.Disable();

        public void EnableRouteFollowing() =>
            _routeFollower.Enable();

        public void DisableRouteFollowing() =>
            _routeFollower.Disable();
    }
}