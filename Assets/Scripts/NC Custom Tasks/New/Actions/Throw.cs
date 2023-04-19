using Gameplay.Character.Player.MonoBehaviour.Events;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Actions
{
    public class Throw : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerEventLauncher> EventLauncher;

        protected override void OnExecute()
        {
            EventLauncher.value.InitiateThrow();
            EndAction(true);
        }
    }
}