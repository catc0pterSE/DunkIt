using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using NodeCanvas.Framework;
using Scene;
using Scene.Ring;
using UnityEngine;
using Utility.Extensions;

namespace z_Test
{
    /*public class MoveAlongAlly : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerMover> Mover;
        [BlackboardOnly] public BBParameter<Ring> EnemyRing;
        [BlackboardOnly] public BBParameter<PlayerFacade> Ally;
        [BlackboardOnly] public BBParameter<CourtDimensions> CourtDimensions;
        [BlackboardOnly] public BBParameter<Transform> HostTransform;
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OppositeTeamFacades;

        public float DistanceDelta;
        public float RotationDistanceThreshold;
        public float MinAngleOffset;
        public float MaxAngleOffset;
        public float AngleToCorrectDirection;
        public float DistanceToCorrectDirection;
        public float DirectionCorrection;

        private Coroutine _moveRoutine;
        private Vector3 _lastAllyPosition;
        private Quaternion _rightRotation;
        private Quaternion _leftRotation;
        private Transform[] _oppositeTeamTransforms;

        private Transform[] OppositeTeamTransforms =>
            _oppositeTeamTransforms ??= OppositeTeamFacades.value.GetTransforms();

        private float RandomOffsetAngle => Random.Range(MinAngleOffset, MaxAngleOffset);

        protected override void OnExecute() //TODO: random, costyl
        {
            _rightRotation = Quaternion.Euler(0, -RandomOffsetAngle, 0);
            _leftRotation = Quaternion.Euler(0, RandomOffsetAngle, 0);
        }

        protected override void OnUpdate()
        {
            Vector3 allyPosition = Ally.value.transform.position;
            Vector3 projectedRingPosition = EnemyRing.value.transform.position;
            projectedRingPosition.y = allyPosition.y;
            Vector3 allyToRing = (projectedRingPosition - allyPosition).normalized;
            CourtDimensions courtDimensions = CourtDimensions.value;

            Quaternion deflection = allyPosition.z > courtDimensions.CourtCenter.z == EnemyRing.value.IsFlipped
                ? _rightRotation
                : _leftRotation;
            Vector3 roughDestination =
                deflection * allyToRing * (Ally.value.MaxPassDistance - DistanceDelta) + allyPosition;
            Vector3 destination = new Vector3
            (
                Mathf.Clamp(roughDestination.x, courtDimensions.XMin, courtDimensions.XMax),
                roughDestination.y,
                Mathf.Clamp(roughDestination.z, courtDimensions.ZMin, courtDimensions.ZMax)
            );

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

                if (needToCorrectDirection == false)
                    continue;
                
                bool enemyIsToTheLeft =
                    Vector3.Cross(direction, enemyPosition - playerPosition).y < 0;

                direction = enemyIsToTheLeft
                    ? Quaternion.Euler(0, DirectionCorrection, 0) * direction
                    : Quaternion.Euler(0, -DirectionCorrection, 0) * direction;
            }

            if (lookingAtAlly)
                Mover.value.MoveLookingAt(direction, allyPosition);
            else
                Mover.value.MoveLookingStraight(direction);
        }
    }*/
}