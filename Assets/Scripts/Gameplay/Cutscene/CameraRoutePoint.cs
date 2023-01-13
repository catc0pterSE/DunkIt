using UnityEngine;

namespace Gameplay.Cutscene
{
    public class CameraRoutePoint: MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private Transform _focusTarget;

        public Transform FocusTarget => _focusTarget;

        public float MovementSpeed => _movementSpeed;

        public void SetFocusTarget(Transform target)
        {
            _focusTarget = target;
        }
    }
}