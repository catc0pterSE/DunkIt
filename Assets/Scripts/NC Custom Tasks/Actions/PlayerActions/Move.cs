using System;
using System.Collections;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;

namespace NC_Custom_Tasks.Actions.PlayerActions
{
    public class Move : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerMover> Mover;
        [BlackboardOnly] public BBParameter<Ring> EnemyRing;
        [BlackboardOnly] public BBParameter<PlayerFacade> Ally;
        [BlackboardOnly] public BBParameter<float> DistanceDelta;
        public float RotationDistanceThreshold;

        private Coroutine _moveRoutine;
        private Vector3 _lastAllyPosition;

        protected override void OnUpdate()
        {
            Ring enemyRing = EnemyRing.value;
            PlayerMover mover = Mover.value;
            PlayerFacade ally = Ally.value;

            float distanceDelta = DistanceDelta.value;
            Vector3 enemyRingPosition = enemyRing.transform.position;
            Vector3 allyPosition = ally.transform.position;
            Vector3 projectedRingPosition = enemyRingPosition;
            projectedRingPosition.y = allyPosition.y;
            Vector3 allyToRing = (projectedRingPosition - allyPosition).normalized;

            Vector3 destination = allyToRing * (ally.MaxPassDistance - distanceDelta) + allyPosition;

            Vector3 playerPosition = mover.transform.position;
            Vector3 path = destination - playerPosition;
            bool lookingAtAlly = path.magnitude < RotationDistanceThreshold;
            Vector3 direction = path.normalized;

            if (lookingAtAlly)
                mover.MoveLookingAt(direction, allyPosition);
            else
                mover.MoveLookingStraight(direction);
        }
    }
}