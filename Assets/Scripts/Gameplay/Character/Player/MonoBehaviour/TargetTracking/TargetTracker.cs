using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.Distance
{
    public class TargetTracker : SwitchableComponent
    {
        [SerializeField] private float _maxThrowDistance;
        [SerializeField] private float _maxDunkDistance;
        [SerializeField] private float _maxPassDistance;

        private Vector3 _enemyRingPosition;
        private Transform _allyTransform;

        public event Action<bool> PassReached;
        public event Action<bool> ThrowReached;
        public event Action<bool> DunkReached;

        public bool IsInThrowDistance => DistanceToEnemyRing < _maxThrowDistance && DistanceToEnemyRing > _maxDunkDistance;
        public bool IsInDunkDistance => DistanceToEnemyRing < _maxDunkDistance;
        public bool IsInPassDistance => DistanceToAlly < _maxPassDistance;

        public float DistanceToEnemyRing => Vector3.Distance(transform.position, _enemyRingPosition);
        public float DistanceToAlly => Vector3.Distance(transform.position, _allyTransform.position);

        public float MaxPassDistance => _maxPassDistance;

        public void Initialize(Vector3 enemyRingPosition, Transform allyTransform)
        {
            _allyTransform = allyTransform;
            _enemyRingPosition = enemyRingPosition;
        }

        private void Update()
        {
            DunkReached?.Invoke(IsInDunkDistance);
            ThrowReached?.Invoke(IsInThrowDistance);
            PassReached?.Invoke(IsInPassDistance);
        }
    }
}