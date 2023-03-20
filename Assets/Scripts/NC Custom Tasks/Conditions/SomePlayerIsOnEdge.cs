using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using Scene;
using UnityEngine;

namespace NC_Custom_Tasks.Conditions
{
    public class SomePlayerIsOnEdge : ConditionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> Player;
        [BlackboardOnly] public BBParameter<CourtDimensions> CourtDimensions;

        protected override bool OnCheck()
        {
            Vector3 playerPosition = Player.value.transform.position;
            CourtDimensions courtDimensions = CourtDimensions.value;

            return playerPosition.z < courtDimensions.ZMin ||
                   playerPosition.z > courtDimensions.ZMax ||
                   playerPosition.x < courtDimensions.XMin ||
                   playerPosition.x > courtDimensions.XMax;
        }
    }
}