using System.Collections.Generic;
using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace NC_Custom_Tasks.Actions
{
    public class CorrectDirection: ActionTask
    {
        [BlackboardOnly] public BBParameter<Transform> PlayerTransform;
        [BlackboardOnly] public BBParameter<List<PlayerFacade>> PlayersToAvoid;
        [BlackboardOnly] public BBParameter<Vector3> Direction;
        [BlackboardOnly] public BBParameter<float> ThreatDistance;
        [BlackboardOnly] public BBParameter<float> CorrectionForce;
        
        protected override void OnExecute()
        {
            Vector3 currentDirection = Direction.value;
            
            foreach (PlayerFacade oppositePlayer in PlayersToAvoid.value)
            {
                Vector3 oppositePlayerPosition = oppositePlayer.transform.position;
                Vector3 playerPosition = PlayerTransform.value.position;

                Vector3 toOppositePlayer = (oppositePlayerPosition - playerPosition);
                Vector3 right = Quaternion.Euler(0,NumericConstants.RightAngle,0)*Direction.value;
                
                float coefficient = Vector3.Dot(right*ThreatDistance.value, (oppositePlayerPosition - playerPosition))/ThreatDistance.value;

                float directionCorrection = coefficient.MapClamped(-1, 1, -CorrectionForce.value, CorrectionForce.value)*(1-toOppositePlayer.magnitude/ThreatDistance.value);
                
                Direction.value = Quaternion.Euler(0, -directionCorrection, 0) * currentDirection;
            }
            
            EndAction(true);
        }
    }
}