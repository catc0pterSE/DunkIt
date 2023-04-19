using NodeCanvas.Framework;
using Scene;
using UnityEngine;

namespace NC_Custom_Tasks.Conditions
{
    public class FreeToGo : ConditionTask
    {
        [BlackboardOnly] public BBParameter<Transform> HostTransform;
        [BlackboardOnly] public BBParameter<Vector3> Direction;
        [BlackboardOnly] public BBParameter<CourtDimensions> CourtDimensions;
        public float Delta;

        protected override bool OnCheck()
        {
            Vector3 pointToCheck = HostTransform.value.position + Direction.value * Delta;
            return CourtDimensions.value.CheckIfPointInsideCourt(pointToCheck);
        }
    }
}