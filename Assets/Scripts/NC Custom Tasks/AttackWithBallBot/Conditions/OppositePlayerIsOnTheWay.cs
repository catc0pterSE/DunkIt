using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;
using Utility.Extensions;

namespace NC_Custom_Tasks.AttackWithBallBot.Conditions
{
    public class OppositePlayerIsOnTheWay : ConditionTask
    {
        [BlackboardOnly] public BBParameter<Transform> PlayerTransform;
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OppositePlayerFacades;
        [BlackboardOnly] public BBParameter<Vector3> TargetPoint;
        public float CheckAngle;
        public float CheckDistance;

        private Transform[] OppositePlayersTransforms => OppositePlayerFacades.value.GetTransforms();
        private Vector3 PlayerPosition => PlayerTransform.value.position;

        protected override bool OnCheck() =>
            OppositePlayersTransforms.FindFirstOrNull(transform =>
            {
                Vector3 oppositePlayerPosition = transform.position;
                return Vector3.Distance(oppositePlayerPosition, PlayerPosition) < CheckDistance
                       && Vector3.Angle(oppositePlayerPosition, TargetPoint.value-PlayerPosition) < CheckAngle;
            }) != null;
    }
}