using NodeCanvas.Framework;
using UnityEngine;

namespace NC_Custom_Tasks.Actions
{
    public class SetDirection: ActionTask
    {
        [BlackboardOnly] public BBParameter<Vector3> Target;
        [BlackboardOnly] public BBParameter<Transform> PlayerTransform;
        [BlackboardOnly] public BBParameter<Vector3> Direction;

        protected override void OnExecute()
        {
            Vector3 playerPosition = PlayerTransform.value.position;
            Vector3 targetPosition = Target.value;
            Vector3 targetProjection = new Vector3(targetPosition.x, playerPosition.y, targetPosition.z);
            Vector3 direction = (targetProjection - playerPosition).normalized;
            Direction.value = direction;
            
            EndAction(true);
        }
    }
}