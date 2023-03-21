using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;
using Utility.Extensions;

namespace NC_Custom_Tasks.Conditions
{
    public class OppositePlayersAreFarEnoughToThrow : ConditionTask
    {
        [BlackboardOnly] public BBParameter<Transform> PlayerTransform;
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OppositePlayerFacades;
        public float AcceptableDistance;

        private Transform[] OppositePlayersTransforms => OppositePlayerFacades.value.GetTransforms();
        private Vector3 PlayerPosition => PlayerTransform.value.position;

        protected override bool OnCheck() =>

            OppositePlayersTransforms.FindFirstOrNull(transform =>
                Vector3.Distance(transform.position, PlayerPosition) < AcceptableDistance) == null;
    }
}