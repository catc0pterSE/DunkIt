using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;
using Utility.Extensions;

namespace NC_Custom_Tasks.AttackWithBallBot.Conditions
{
    public class ClearToPass : ConditionTask
    {
        [BlackboardOnly] public BBParameter<Transform> PlayerTransform;
        [BlackboardOnly] public BBParameter<PlayerFacade> Ally;
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OppositeTeamFacades;
        public float AcceptableDistance;


        private Vector3[] OppositePlayerPositions => OppositeTeamFacades.value.GetTransforms().GetTransformPositions();

        protected override bool OnCheck()
        {
            Vector3 allyPosition = Ally.value.transform.position;
            Vector3 playerPosition = PlayerTransform.value.position;
            Vector3 playerToAllyDirection = (allyPosition - playerPosition).normalized;

            foreach (Vector3 oppositePlayerPosition in OppositePlayerPositions)
            {
                Vector3 playerToOppositePlayer = oppositePlayerPosition - playerPosition;
                float distanceToClosestPoint = Vector3.Dot(playerToOppositePlayer, playerToAllyDirection);
                Vector3 closestPoint = playerPosition + playerToAllyDirection * distanceToClosestPoint;

                Vector3 playerToClosestPoint = closestPoint - playerPosition;
                Vector3 allyToClosestPoint = closestPoint - allyPosition;
                
                bool oppositePlayerIsBetween = Vector3.Dot(playerToAllyDirection, playerToClosestPoint) > 0
                                               && Vector3.Dot(-playerToAllyDirection, allyToClosestPoint) > 0;

                if (oppositePlayerIsBetween == false)
                    continue;
                
                Vector3 oppositePlayerToClosestPoint = closestPoint - oppositePlayerPosition;

                if (oppositePlayerToClosestPoint.magnitude < AcceptableDistance)
                    return false;
            }

            return true;
        }
    }
}