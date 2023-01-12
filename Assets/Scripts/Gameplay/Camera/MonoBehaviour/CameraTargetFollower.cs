using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Camera.MonoBehaviour
{
    public class CameraTargetFollower: SwitchableComponent
    {
        [SerializeField] private float _trackingSpeed = 5;
        [SerializeField] private float _projectionDistanceToPlayer = 17;
        [SerializeField] private float _cameraHeight = 5;

        private Transform _target;

        private Vector3 TargetPosition => _target.position;

        private void FixedUpdate()
        {
            MoveToPlayer();
        }
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }
        
        private void MoveToPlayer()
        {
            transform.position = Vector3.Lerp(transform.position, GetDirection(), _trackingSpeed * Time.deltaTime);
        }

        private Vector3 GetDirection()
        {
            Vector3 projectionPosition = TargetPosition - GetNormalizedForwardProjection() * _projectionDistanceToPlayer;
            float yPosition = TargetPosition.y + _cameraHeight;
            
            return new Vector3(projectionPosition.x, yPosition, projectionPosition.z);
        }

        private Vector3 GetNormalizedForwardProjection()
        {
            Vector3 forward = transform.forward;
            forward.y = 0;
            forward.Normalize();
            
            return forward;
        }
    }
}