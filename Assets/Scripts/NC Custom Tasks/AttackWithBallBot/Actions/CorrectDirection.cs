using System.Collections.Generic;
using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;

namespace NC_Custom_Tasks.AttackWithBallBot.Actions
{
    public class CorrectDirection: ActionTask
    {
        [BlackboardOnly] public BBParameter<Transform> PlayerTransform;
        [BlackboardOnly] public BBParameter<List<PlayerFacade>> PlayersToAvoid;
        [BlackboardOnly] public BBParameter<Vector3> Direction;
        public float DirectionCorrection;
        
        protected override void OnExecute()
        {
            Vector3 currentDirection = Direction.value;
            
            foreach (PlayerFacade oppositePlayer in PlayersToAvoid.value)
            {
                Vector3 oppositePlayerPosition = oppositePlayer.transform.position;
                Vector3 playerPosition = PlayerTransform.value.position;
            
                bool enemyIsToTheLeft =
                    Vector3.Cross(currentDirection, oppositePlayerPosition - playerPosition).y < 0;

                Direction.value = enemyIsToTheLeft
                    ? Quaternion.Euler(0, DirectionCorrection, 0) * currentDirection
                    : Quaternion.Euler(0, -DirectionCorrection, 0) * currentDirection;
            }
            
            EndAction(true);
        }
    }
}