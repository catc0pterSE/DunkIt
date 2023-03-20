using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;

namespace NC_Custom_Tasks.Actions
{
    
    public class SelectTargetPlayer: ActionTask
    {
        [BlackboardOnly] public BBParameter<Transform> HostTransform;
        [BlackboardOnly] public BBParameter<PlayerFacade> TargetPlayer;
        [BlackboardOnly] public BBParameter<Vector3> Target;
        public float Offset;
        protected override void OnExecute()
        {
            Vector3 playerPosition = HostTransform.value.position;
            Vector3 targetPlayerPosition = TargetPlayer.value.transform.position;
            Vector3 toTarget = targetPlayerPosition - playerPosition;
            float distance = toTarget.magnitude - Offset;
            toTarget.Normalize();

            Target.value = playerPosition + toTarget * distance;
            EndAction(true);
        }
    }
}