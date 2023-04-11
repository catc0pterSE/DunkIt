using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Actions
{
    public class Pass : ActionTask
    {
        [BlackboardOnly] public BBParameter<AIControlledBrain> AIControlledEventLauncher;

        protected override void OnExecute()
        {
            
            EndAction(true);
        }
    }
}