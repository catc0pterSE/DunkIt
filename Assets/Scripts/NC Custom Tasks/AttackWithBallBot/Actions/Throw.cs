using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Gameplay.Character.Player.MonoBehaviour.Brains;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.AttackWithBallBot.Actions
{
    public class Throw : ActionTask
    {
        [BlackboardOnly] public BBParameter<AIControlledEventLauncher> AIControlledEventLauncher;
        [BlackboardOnly] public BBParameter<BallThrower> BallThrower;
        
        protected override void OnExecute()
        {
            AIControlledEventLauncher.value.InitiateThrow();
        }
    }
}