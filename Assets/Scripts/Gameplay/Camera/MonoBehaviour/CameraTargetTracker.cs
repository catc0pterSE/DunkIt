using UnityEngine;

namespace Gameplay.Camera
{
    public class CameraTargetTracker: MonoBehaviour
    {
        [SerializeField] private float _trackingSpeed = 5;
        [SerializeField] private float _projectionDistanceToPlayer = 17;
        [SerializeField] private float _cameraHeight = 5;

        private Transform _target;

        private void FixedUpdate()
        {
            MoveToPlayer();
        }
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public Vector3 CalculateCameraRelativeDirection(Vector2 inputDirection)
        {
            Vector3 direction = transform.TransformDirection(inputDirection.x, 0, inputDirection.y);

            return direction;
        }
        
        private void MoveToPlayer()
        {
            transform.position = Vector3.Lerp(transform.position, GetDirection(), _trackingSpeed * Time.deltaTime);
        }

        private Vector3 GetDirection()
        {
            Vector3 targetPosition = _target.position;
            Vector3 projectionPosition = targetPosition - GetNormalizedForwardProjection() * _projectionDistanceToPlayer;
            float yPosition = targetPosition.y + _cameraHeight;
            
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