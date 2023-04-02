using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using Utility.Extensions;

namespace NC_Custom_Tasks.Actions
{
    public class SetOppositePlayers : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OppositeTeamFacades;
        [BlackboardOnly] public BBParameter<PlayerFacade> OppositePlayerWithoutBall;
        [BlackboardOnly] public BBParameter<PlayerFacade> OppositePlayerWithBall;

        protected override void OnExecute()
        {
            OppositePlayerWithBall.value = OppositeTeamFacades.value.FindFirstOrNull(player => player.OwnsBall);
            OppositePlayerWithoutBall.value = OppositeTeamFacades.value.FindFirstOrNull(player => player.OwnsBall == false);
            EndAction(true);
        }
    }
}