using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Actions.PlayerActions
{
    public class Idle : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> Host;

        protected override void OnUpdate() =>
            Host.value.RotateToAlly();
    }
}