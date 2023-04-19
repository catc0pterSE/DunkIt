using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using Gameplay.Character.Player.MonoBehaviour.Events;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Actions
{
    public class Pass : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerEventLauncher> EventLauncher;

        protected override void OnExecute()
        {
            EventLauncher.value.InitiatePass();
            EndAction(true);
        }
    }
}