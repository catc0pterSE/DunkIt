using System.Collections.Generic;
using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;

namespace NC_Custom_Tasks.Actions
{
    public class CalculateThreat : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OppositePlayersFacades;
        [BlackboardOnly] public BBParameter<List<PlayerFacade>> PlayersToAvoid;
        [BlackboardOnly] public BBParameter<Transform> HostTransform;
        [BlackboardOnly] public BBParameter<float> ThreatDistance;
        [BlackboardOnly] public BBParameter<float> Threat;

        protected override void OnExecute()
        {
            float threat = 0;
            Vector3 playerPosition = HostTransform.value.position;

            foreach (PlayerFacade oppositePlayer in PlayersToAvoid.value)
            {
                threat += 1 - (oppositePlayer.transform.position - playerPosition).magnitude / ThreatDistance.value;
            }

            Threat.value = threat;
            EndAction(true);
        }
    }
}