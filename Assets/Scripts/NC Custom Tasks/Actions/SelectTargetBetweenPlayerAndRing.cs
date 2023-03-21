using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;

namespace NC_Custom_Tasks.Actions
{
    public class SelectTargetBetweenPlayerAndRing : ActionTask
    {
        [BlackboardOnly] public BBParameter<Ring> Ring;
        [BlackboardOnly] public BBParameter<PlayerFacade> Player;
        [BlackboardOnly] public BBParameter<Vector3> Target;
        public float OffsetFromRingPercents;

        protected override void OnExecute()
        {
            Vector3 ringPosition = Ring.value.transform.position;
            Vector3 ringToPlayer = (Player.value.transform.position - ringPosition);
            float distance = ringToPlayer.magnitude;
            ringToPlayer.Normalize();
            Target.value = ringPosition + ringToPlayer * (distance * OffsetFromRingPercents)/NumericConstants.MaxPercents;
            EndAction(true);
        }
    }
}