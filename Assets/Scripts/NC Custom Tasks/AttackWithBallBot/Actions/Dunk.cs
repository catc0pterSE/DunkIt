using Gameplay.Character.Player.MonoBehaviour.BallHandle.Dunk;
using Gameplay.Character.Player.MonoBehaviour.Brains;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.AttackWithBallBot.Actions
{
    public class Dunk : ActionTask
    {
        [BlackboardOnly] public BBParameter<AIControlledEventLauncher> AIControlledEventLauncher;
        [BlackboardOnly] public BBParameter<Dunker> Dunker;

        protected override void OnExecute()
        {
            AIControlledEventLauncher.value.InitiateDunk();
            Dunker.value.Dunk();
            EndAction(true);
        }
    }
}