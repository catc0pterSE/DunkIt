using System;
using System.Linq;
using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Actions
{
    public class SetAllies: ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade[]> Allies;
        [BlackboardOnly] public BBParameter<PlayerFacade> AllyWithBall;
        [BlackboardOnly] public BBParameter<PlayerFacade[]> AlliesWithoutBall;

        protected override void OnExecute()
        {
            AllyWithBall.value = Allies.value.FirstOrDefault(ally => ally.OwnsBall) ??
                                 throw new Exception("No allies with ball");
            AlliesWithoutBall.value = Allies.value.Where(ally => ally != AllyWithBall.value).ToArray();
            
            EndAction(true);
        }
    }
}