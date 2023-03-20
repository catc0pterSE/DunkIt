using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;

namespace NC_Custom_Tasks.Conditions
{
    public class SomePlayerCloserToRing: ConditionTask
    {
        [BlackboardOnly] public BBParameter<Transform> HostTransform;
        [BlackboardOnly] public BBParameter<PlayerFacade> OtherPlayerFacade;
        [BlackboardOnly] public BBParameter<Ring> Ring;

        protected override bool OnCheck()
        {
            Vector3 ringPosition = Ring.value.transform.position;
            return (Vector3.Distance(HostTransform.value.position, ringPosition) >
                    Vector3.Distance(OtherPlayerFacade.value.transform.position, ringPosition));
        }
    }
}