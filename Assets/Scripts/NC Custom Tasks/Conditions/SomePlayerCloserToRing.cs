using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;

namespace NC_Custom_Tasks.Conditions
{
    public class SomePlayerCloserToRing: ConditionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> Player1;
        [BlackboardOnly] public BBParameter<PlayerFacade> Player2;
        [BlackboardOnly] public BBParameter<Ring> Ring;

        protected override bool OnCheck()
        {
            Vector3 ringPosition = Ring.value.transform.position;
            return (Vector3.Distance(Player1.value.transform.position, ringPosition) <
                    Vector3.Distance(Player2.value.transform.position, ringPosition));
        }
    }
}