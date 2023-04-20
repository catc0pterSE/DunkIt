using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Actions
{
    public class IdleLookingAtPlayer : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> Host;
        [BlackboardOnly] public BBParameter<PlayerFacade> PlayerToLookAt;

        protected override void OnExecute()
        {
            Host.value.RotateTo(PlayerToLookAt.value.transform.position); 
            EndAction(true);
        }
    }
}