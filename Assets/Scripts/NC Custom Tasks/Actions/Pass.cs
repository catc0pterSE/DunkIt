using Gameplay.Character.Player.MonoBehaviour.Brains;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Actions
{
    public class Pass : ActionTask
    {
        [BlackboardOnly] public BBParameter<AIControlledEventLauncher> AIControlledEventLauncher;

        protected override void OnExecute()
        {
            AIControlledEventLauncher.value.InitiatePass();
            EndAction(true);
        }
    }
}