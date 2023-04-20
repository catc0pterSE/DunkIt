using System.Linq;
using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using Scene.Ring;
using Utility.Extensions;

namespace NC_Custom_Tasks.New.Conditions
{
    public class SomePlayerClosestSomeRing : ConditionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> PlayerToCheck;
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OtherPlayers;
        [BlackboardOnly] public BBParameter<Ring> RingToCheck;

        protected override bool OnCheck()
        {
            var players = OtherPlayers.value.Append(PlayerToCheck.value);
            return PlayerToCheck.value == players.FindClosest(RingToCheck.value.transform.position);
        }
    }
}