using Gameplay.Character.Player.MonoBehaviour.BallHandle.Dunk;
using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Actions
{
    public class Dunk : ActionTask
    {
        [BlackboardOnly] public BBParameter<AIControlledBrain> AIControlledEventLauncher;
        [BlackboardOnly] public BBParameter<Dunker> Dunker;

        protected override void OnExecute()
        {
            
            EndAction(true);
        }
    }
}