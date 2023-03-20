using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using Scene;
using Scene.Ring;
using UnityEngine;

namespace NC_Custom_Tasks.Conditions
{
    public class SomePlayerIsOnOurHalf : ConditionTask
    {
        [BlackboardOnly] public BBParameter<CourtDimensions> CourtDimensions;
        [BlackboardOnly] public BBParameter<Ring> PlayerRing;
        [BlackboardOnly] public BBParameter<PlayerFacade> PlayerToCHeck;

        protected override bool OnCheck()
        {
            Vector3 courtCenter = CourtDimensions.value.CourtCenter;
            Vector3 playerToCheckPosition = PlayerToCHeck.value.transform.position;
            bool isPlayerSideLeft = courtCenter.x > PlayerRing.value.transform.position.x;

            return isPlayerSideLeft ? playerToCheckPosition.x < courtCenter.x : playerToCheckPosition.x > courtCenter.x;
        }
    }
}