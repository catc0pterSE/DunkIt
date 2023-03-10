using System.Numerics;
using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using Utility.Constants;
using Vector3 = UnityEngine.Vector3;

namespace NC_Custom_Tasks.Conditions.PlayerConditions
{
    public class AllyIsOnOptimalPassRange : ConditionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> Ally;
        [BlackboardOnly] public BBParameter<PlayerFacade> Host;
        [BlackboardOnly] public BBParameter<float> DistanceTolerance;

        protected override bool OnCheck()
        {
            PlayerFacade host = Host.value;
            PlayerFacade ally = Ally.value;
            float distanceTolerance = DistanceTolerance.value;

            float distanceToAlly = Vector3.Distance(ally.transform.position, host.transform.position);

            return distanceToAlly > ally.MaxPassDistance - distanceTolerance * NumericConstants.Double &&
                   distanceToAlly < ally.MaxPassDistance - distanceTolerance;
        }
    }
}