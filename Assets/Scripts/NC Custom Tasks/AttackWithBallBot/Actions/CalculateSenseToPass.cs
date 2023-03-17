using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Distance;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;

namespace NC_Custom_Tasks.AttackWithBallBot.Actions
{
    public class CalculateSenseToPass : ActionTask
    {
        [BlackboardOnly] public BBParameter<TargetTracker> HostTargetTracker;
        [BlackboardOnly] public BBParameter<Transform> HostTransform;
        [BlackboardOnly] public BBParameter<Ring> OppositeRing;
        [BlackboardOnly] public BBParameter<PlayerFacade> Ally;
        [BlackboardOnly] public BBParameter<float> SenseToPass;
        public float DifferenceForMaxSense;

        protected override void OnExecute()
        {
            Vector3 oppositeRingPosition = OppositeRing.value.transform.position;
            float allyToRingDistance = (oppositeRingPosition-Ally.value.transform.position).magnitude;
            float playerToRingDistance = (oppositeRingPosition-HostTransform.value.position).magnitude;
            float difference = Mathf.Clamp( playerToRingDistance-allyToRingDistance, 0,
                DifferenceForMaxSense);

            SenseToPass.value = difference / DifferenceForMaxSense;
            EndAction(true);
        }
    }
}