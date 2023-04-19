using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.TargetTracking;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;

namespace NC_Custom_Tasks.Actions
{
    public class CalculateSenseToPass : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> PassTarget;
        [BlackboardOnly] public BBParameter<Transform> HostTransform;
        [BlackboardOnly] public BBParameter<Ring> OppositeRing;
        [BlackboardOnly] public BBParameter<float> SenseToPass;
        public float DistanceToRingDeltaForMaxSense;

        protected override void OnExecute()
        {
            Vector3 oppositeRingPosition = OppositeRing.value.transform.position;
            float allyToRingDistance = Vector3.Distance(oppositeRingPosition,PassTarget.value.transform.position);
            float playerToRingDistance = Vector3.Distance(oppositeRingPosition,HostTransform.value.position);
            float difference = Mathf.Clamp( playerToRingDistance-allyToRingDistance, 0, DistanceToRingDeltaForMaxSense);

            SenseToPass.value = difference / DistanceToRingDeltaForMaxSense;
            EndAction(true);
        }
    }
}