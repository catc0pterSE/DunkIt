using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;
using Utility.Extensions;

namespace NC_Custom_Tasks.Conditions
{
    public class IsSomePlayerFarEnoughFromEnemies: ConditionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> PlayerToCheck;
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OppositePlayerFacades;
        public float AcceptableDistance;

        private Transform[] OppositePlayersTransforms => OppositePlayerFacades.value.GetTransforms();
        private Vector3 AllyPosition => PlayerToCheck.value.transform.position;

        protected override bool OnCheck() =>
            OppositePlayersTransforms.FindFirstOrNull(transform =>
                Vector3.Distance(transform.position, AllyPosition) < AcceptableDistance) == null;
    }
}