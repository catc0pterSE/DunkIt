
using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using Scene;
using Scene.Ring;
using UnityEngine;

namespace NC_Custom_Tasks.Actions
{
    public class SelectTargetAlongAlly : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> Ally;
        [BlackboardOnly] public BBParameter<Ring> OppositeRing;
        [BlackboardOnly] public BBParameter<CourtDimensions> CourtDimensions;
        [BlackboardOnly] public BBParameter<Vector3> TargetAlongAlly;
        [BlackboardOnly] public BBParameter<float> OffsetAngle;
        public float DistanceDelta;

        protected override void OnExecute()
        {
            Vector3 allyPosition = Ally.value.transform.position;
            Vector3 projectedRingPosition = OppositeRing.value.transform.position;
            projectedRingPosition.y = allyPosition.y;
            Vector3 allyToRing = (projectedRingPosition - allyPosition).normalized;
            CourtDimensions courtDimensions = CourtDimensions.value;
            
            Quaternion deflection = allyPosition.z > courtDimensions.CourtCenter.z == OppositeRing.value.IsFlipped
                ? Quaternion.Euler(0, -OffsetAngle.value, 0)
                : Quaternion.Euler(0, OffsetAngle.value, 0);
            
            Vector3 roughDestination = deflection * allyToRing * (Ally.value.MaxPassDistance - DistanceDelta) + allyPosition;
            
            Vector3 destination = new Vector3
            (
                Mathf.Clamp(roughDestination.x, courtDimensions.XMin, courtDimensions.XMax),
                roughDestination.y,
                Mathf.Clamp(roughDestination.z, courtDimensions.ZMin, courtDimensions.ZMax)
            );

            TargetAlongAlly.value = destination;
            
            EndAction(true);
        }
    }
}