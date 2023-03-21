using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;

namespace NC_Custom_Tasks.Conditions
{
    public class SomePlayerCloserToAnother : ConditionTask
    {
        [BlackboardOnly] public BBParameter<Transform> HostTransform;
        [BlackboardOnly] public BBParameter<PlayerFacade> OtherPlayer;
        [BlackboardOnly] public BBParameter<PlayerFacade> Target;

        protected override bool OnCheck()
        {
            Vector3 playerPosition = HostTransform.value.transform.position;
            Vector3 otherPlayerPosition = OtherPlayer.value.transform.position;
            Vector3 targetPosition = Target.value.transform.position;

            return Vector3.Distance(otherPlayerPosition, targetPosition) <
                   Vector3.Distance(playerPosition, targetPosition);
        }
    }
}