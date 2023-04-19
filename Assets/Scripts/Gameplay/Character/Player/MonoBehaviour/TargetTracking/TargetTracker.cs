﻿using System;
using System.Linq;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour.TargetTracking
{
    public class TargetTracker : SwitchableComponent
    {
        [SerializeField] private float _maxThrowDistance = 12f;
        [SerializeField] private float _maxDunkDistance = 5f;
        [SerializeField] private float _maxPassDistance = 12f;

        private Vector3 _enemyRingPosition;
        private PlayerFacade[] _allies;

        public void Initialize(Vector3 enemyRingPosition, PlayerFacade[] allies)
        {
            _allies = allies;
            _enemyRingPosition = enemyRingPosition;
        }

        public event Action<bool> PassReached;
        public event Action<bool> ThrowReached;
        public event Action<bool> DunkReached;

        public bool IsInThrowDistance => DistanceToEnemyRing < _maxThrowDistance && DistanceToEnemyRing > _maxDunkDistance;
        public bool IsInDunkDistance => DistanceToEnemyRing < _maxDunkDistance;
        public PlayerFacade PassTarget { get; private set; }
        public bool CanPass => PassTarget != null;
        public float MaxPassDistance => _maxPassDistance;
        public float MaxThrowDistance => _maxThrowDistance;
        public float MaxDunkDistance => _maxDunkDistance;
        private float DistanceToEnemyRing => Vector3.Distance(transform.position, _enemyRingPosition);

        private void Update()
        {
            DunkReached?.Invoke(IsInDunkDistance);
            ThrowReached?.Invoke(IsInThrowDistance);
            SetPassTarget();
            PassReached?.Invoke(CanPass);
        }
        
        private void SetPassTarget() => 
            PassTarget = TrySelectAllyToPass(out PlayerFacade passTarget) ? passTarget : null;
        private bool TrySelectAllyToPass(out PlayerFacade passTarget)
        {
            passTarget = null;

            Vector3 playerPosition = transform.position;
            PlayerFacade[] alliesInPassRange = _allies.Where(ally =>
                Vector3.Distance(playerPosition, ally.transform.position) <= _maxPassDistance).ToArray();

            if (alliesInPassRange.Any() == false)
                return false;

            passTarget = alliesInPassRange.FindClosestToDirection(transform.forward, playerPosition);

            return true;
        }
    }
}