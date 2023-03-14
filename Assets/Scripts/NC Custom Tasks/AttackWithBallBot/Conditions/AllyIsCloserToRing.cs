using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;

namespace NC_Custom_Tasks.AttackWithBallBot.Conditions
{
    public class AllyIsCloserToRing: ConditionTask
    {
        [BlackboardOnly] public BBParameter<Transform> PlayerTransform;
        [BlackboardOnly] public BBParameter<PlayerFacade> Ally;
        [BlackboardOnly] public BBParameter<Ring> OppositeRing;
        public float SignificantGap;

        protected override bool OnCheck()
        {
            Vector3 oppositeRingPosition = OppositeRing.value.transform.position;
            Vector3 allyPosition = Ally.value.transform.position;
            Vector3 playerPosition = PlayerTransform.value.position;

            Vector3 allyToRing = oppositeRingPosition - allyPosition;
            Vector3 playerToRing = oppositeRingPosition - playerPosition;

            return playerToRing.magnitude - allyToRing.magnitude >= SignificantGap;
        }
    }
}