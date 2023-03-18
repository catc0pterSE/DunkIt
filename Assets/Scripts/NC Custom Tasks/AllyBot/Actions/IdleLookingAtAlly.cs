using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.AllyBot.Actions
{
    public class IdleLookingAtAlly : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> Host;

        protected override void OnUpdate() =>
            Host.value.RotateToAlly();
    }
}