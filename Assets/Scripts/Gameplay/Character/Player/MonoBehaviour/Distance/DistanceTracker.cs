using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.Distance
{
    public class DistanceTracker : SwitchableComponent
    {
        [SerializeField] private float _maxThrowDistance;
        [SerializeField] private float _maxDunkDistance;
        [SerializeField] private float _maxPassDistance;

        private Vector3 _enemyRingPosition;
        private Transform _allyTransform;

        public event Action<bool> PassReached;
        public event Action<bool> ThrowReached;
        public event Action<bool> DunkReached;

        public bool InThrowZone => DistanceToEnemyRing < _maxThrowDistance && DistanceToEnemyRing > _maxDunkDistance;
        public bool IsInDunkZone => DistanceToEnemyRing < _maxDunkDistance;
        public bool IsPassPossible => DistanceToAlly < _maxPassDistance;

        private float DistanceToEnemyRing => Vector3.Distance(transform.position, _enemyRingPosition);
        private float DistanceToAlly => Vector3.Distance(transform.position, _allyTransform.position);

        public void Initialize(Vector3 enemyRingPosition, Transform allyTransform)
        {
            _allyTransform = allyTransform;
            _enemyRingPosition = enemyRingPosition;
        }

        private void FixedUpdate()
        {
            DunkReached?.Invoke(IsInDunkZone);
            ThrowReached?.Invoke(InThrowZone);
            PassReached?.Invoke(IsPassPossible);
        }
    }
}