using NodeCanvas.Framework;
using Scene;
using Scene.Ring;
using UnityEngine;

namespace NC_Custom_Tasks.AttackWithBallBot.Actions
{
    public abstract class SelectTarget : ActionTask
    {
        [BlackboardOnly] public BBParameter<Ring> OppositeRing;
        [BlackboardOnly] public BBParameter<CourtDimensions> CourtDimensions;
        [BlackboardOnly] public BBParameter<Vector3> Target;

        protected float Distance;

        protected override void OnExecute()
        {
            CourtDimensions courtDimensions = CourtDimensions.value;
            Vector3 ringPosition = OppositeRing.value.transform.position;
            Vector2 randomInCircle = Random.insideUnitCircle * Distance;

            Vector3 target = new Vector3
            (
                Mathf.Clamp(ringPosition.x+randomInCircle.x, courtDimensions.XMin, courtDimensions.XMax),
                ringPosition.y,
                Mathf.Clamp(ringPosition.z+randomInCircle.y, courtDimensions.ZMin, courtDimensions.ZMax)
            );

            Target.value = target;

            EndAction(true);
        }
    }
}