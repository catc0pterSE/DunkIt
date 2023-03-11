using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;
using Utility.Extensions;

namespace NC_Custom_Tasks.Actions.PlayerActions
{
    public class MoveToRing : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerMover> Mover;
        [BlackboardOnly] public BBParameter<Ring> EnemyRing;
        [BlackboardOnly] public BBParameter<PlayerFacade> Ally;
        [BlackboardOnly] public BBParameter<Transform> CourtCenter;
        [BlackboardOnly] public BBParameter<Transform> HostTransform;
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OppositeTeamFacades;

        public float DistanceDelta;
        public float RotationDistanceThreshold;
        public float AngleOffset;
        public float AngleToCorrectDirection;
        public float DistanceToCorrectDirection;
        public float DirectionCorrection;

        private Coroutine _moveRoutine;
        private Vector3 _lastAllyPosition;
        private Quaternion _leftDeflection;
        private Quaternion _rightDeflection;
        private Transform[] _oppositeTeamTransforms;

        private Transform[] OppositeTeamTransforms =>
            _oppositeTeamTransforms ??= OppositeTeamFacades.value.GetTransforms();

        protected override void OnExecute()
        {
            _leftDeflection = Quaternion.Euler(0, AngleOffset, 0);
            _rightDeflection = Quaternion.Euler(0, -AngleOffset, 0);
        }

        protected override void OnUpdate()
        {
            Vector3 allyPosition = Ally.value.transform.position;
            Vector3 projectedRingPosition = EnemyRing.value.transform.position;
            projectedRingPosition.y = allyPosition.y;
            Vector3 allyToRing = (projectedRingPosition - allyPosition).normalized;

            Quaternion deflection = allyPosition.z > CourtCenter.value.position.z ? _leftDeflection : _rightDeflection;
            Vector3 destination = deflection * allyToRing * (Ally.value.MaxPassDistance - DistanceDelta) + allyPosition;

            Vector3 playerPosition = HostTransform.value.position;
            Vector3 path = destination - playerPosition;
            bool lookingAtAlly = path.magnitude < RotationDistanceThreshold;
            Vector3 direction = path.normalized;

            foreach (Transform enemyTransform in OppositeTeamTransforms)
            {
                Vector3 enemyPosition = enemyTransform.position;
                Vector3 directionToEnemy = enemyPosition - playerPosition;
                
                bool needToCorrectDirection =
                    Vector3.Angle(direction, directionToEnemy) < AngleToCorrectDirection
                    && Vector3.Distance(playerPosition, enemyPosition) < DistanceToCorrectDirection;

                if (needToCorrectDirection)
                    direction = Quaternion.Euler(0, DirectionCorrection, 0) * direction;
            }

            if (lookingAtAlly)
                Mover.value.MoveLookingAt(direction, allyPosition);
            else
                Mover.value.MoveLookingStraight(direction);
        }
    }
}