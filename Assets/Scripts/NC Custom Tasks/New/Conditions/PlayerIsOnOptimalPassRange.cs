using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;
using Utility.Constants;
using Vector3 = UnityEngine.Vector3;

namespace NC_Custom_Tasks.Conditions
{
    public class PlayerIsOnOptimalPassRange : ConditionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> PlayerToCheck;
        [BlackboardOnly] public BBParameter<Transform> HostTransform;
        public float DistanceTolerance;

        protected override bool OnCheck()
        {
            PlayerFacade ally = PlayerToCheck.value;;

            float distanceToAlly = Vector3.Distance(ally.transform.position, HostTransform.value.position);

            return distanceToAlly > ally.MaxPassDistance - DistanceTolerance * NumericConstants.Double &&
                   distanceToAlly < ally.MaxPassDistance - DistanceTolerance;
        }
    }
}