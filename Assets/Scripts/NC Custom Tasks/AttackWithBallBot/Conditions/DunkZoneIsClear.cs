using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Distance;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;
using Utility.Extensions;

namespace NC_Custom_Tasks.AttackWithBallBot.Conditions
{
    public class DunkZoneIsClear : ConditionTask
    {
        [BlackboardOnly] public BBParameter<Transform> PlayerTransform;
        [BlackboardOnly] public BBParameter<TargetTracker> TargetTracker;
        [BlackboardOnly] public BBParameter<Ring> EnemyRing;
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OppositeTeamFacades;

        private Vector3[] OppositeTeamPositions => OppositeTeamFacades.value.GetTransforms().GetTransformPositions();

        protected override bool OnCheck()
        {
            Vector3 playerPosition = PlayerTransform.value.position;
            float maxDunkDistance = TargetTracker.value.MaxDunkDistance;
            Transform enemyRingTransform = EnemyRing.value.transform;
            Vector3 enemyRingPosition = enemyRingTransform.position;
            Vector3 enemyRingForward = enemyRingTransform.forward;


            Vector3 offset = EnemyRing.value.IsFlipped
                ? -enemyRingForward * maxDunkDistance
                : enemyRingForward * maxDunkDistance;

            Vector3 rightExtremePoint = enemyRingPosition + offset;
            Vector3 leftExtremePoint = enemyRingPosition - offset;

            Vector3 playerToRightExtremePoint = rightExtremePoint - playerPosition;
            Vector3 playerToLeftExtremePoint = leftExtremePoint - playerPosition;

            foreach (Vector3 oppositePlayerPosition in OppositeTeamPositions)
            {
                Vector3 playerToOppositePlayer = oppositePlayerPosition - playerPosition;

                bool oppositePlayerIsOnTheWay =
                    Vector3.Cross(playerToRightExtremePoint, playerToOppositePlayer).y < 0 &&
                    Vector3.Cross(playerToLeftExtremePoint, playerToOppositePlayer).y > 0;

                if (oppositePlayerIsOnTheWay)
                    return false;
            }

            return true;
        }
    }
}