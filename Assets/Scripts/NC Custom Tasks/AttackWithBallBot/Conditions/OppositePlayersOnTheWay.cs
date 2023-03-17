using System.Collections.Generic;
using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;

namespace NC_Custom_Tasks.AttackWithBallBot.Conditions
{
    public class OppositePlayersOnTheWay : ConditionTask
    {
        [BlackboardOnly] public BBParameter<Transform> PlayerTransform;
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OppositePlayerFacades;
        [BlackboardOnly] public BBParameter<Vector3> Direction;
        [BlackboardOnly] public BBParameter<List<PlayerFacade>> PlayersToAvoid;
        [BlackboardOnly] public BBParameter<float> ThreatDistance;
        public float CheckAngle;
        
        private Vector3 PlayerPosition => PlayerTransform.value.position;

        protected override bool OnCheck()
        {
            PlayersToAvoid.value.Clear();

            foreach (PlayerFacade oppositePlayer in OppositePlayerFacades.value)
            {
                Vector3 oppositePlayerPosition = oppositePlayer.transform.position;
                bool oppositePlayerOnTheWay =
                    Vector3.Distance(oppositePlayerPosition, PlayerPosition) < ThreatDistance.value
                    && Vector3.Angle(oppositePlayerPosition, Direction.value) < CheckAngle;

                if (oppositePlayerOnTheWay)
                {
                    Debug.DrawLine(PlayerPosition, oppositePlayerPosition, Color.red);
                    PlayersToAvoid.value.Add(oppositePlayer);
                }
            }

            return PlayersToAvoid.value.Count > 0;
        }
    }
}