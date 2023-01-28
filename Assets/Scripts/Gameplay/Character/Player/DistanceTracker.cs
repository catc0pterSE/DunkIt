using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player
{
    public class DistanceTracker: SwitchableMonoBehaviour
    {
        [SerializeField] private float _maxThrowDistance;
        [SerializeField] private float _maxDunkDistance;

        private Vector3 _enemyRingPosition;

        public event Action<bool> ThrowReached;
        public event Action<bool> DunkReached;

        public bool InThrowZone => DistanceToEnemyRing < _maxThrowDistance && DistanceToEnemyRing>_maxDunkDistance;
        public bool InDunkZone => DistanceToEnemyRing < _maxDunkDistance;
        private float DistanceToEnemyRing => Vector3.Distance(transform.position, _enemyRingPosition);

        public void Initialize(Vector3 enemyRingPosition)
        {
            _enemyRingPosition = enemyRingPosition;
        }
        
       private void FixedUpdate()
       {
           DunkReached?.Invoke(InDunkZone);
           ThrowReached?.Invoke(InThrowZone);
       }
    }
}