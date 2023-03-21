using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;
using Utility.Constants;

namespace NC_Custom_Tasks.Actions
{
    public class SelectTargetBetweenPlayers: ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> Player1;
        [BlackboardOnly] public BBParameter<PlayerFacade> Player2;
        [BlackboardOnly] public BBParameter<Vector3> Target;
        public float OffsetFromPlayer1Percents;

        protected override void OnExecute()
        {
            Vector3 player1Position = Player1.value.transform.position;
            Vector3 player1ToPlayer2 = (Player2.value.transform.position - player1Position);
            float distance = player1ToPlayer2.magnitude;
            player1ToPlayer2.Normalize();
            
            Target.value = player1Position + player1ToPlayer2 * (distance * OffsetFromPlayer1Percents)/NumericConstants.MaxPercents;
            EndAction(true);
        }
    }
}