using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;
using Utility.Extensions;

namespace NC_Custom_Tasks.AttackWithBallBot.Conditions
{
    public class AllyIsFarEnoughFromEnemies: ConditionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> Ally;
        [BlackboardOnly] public BBParameter<PlayerFacade[]> OppositePlayerFacades;
        public float AcceptableDistance;

        private Transform[] OppositePlayersTransforms => OppositePlayerFacades.value.GetTransforms();
        private Vector3 AllyPosition => Ally.value.transform.position;

        protected override bool OnCheck() =>
            OppositePlayersTransforms.FindFirstOrNull(transform =>
                Vector3.Distance(transform.position, AllyPosition) < AcceptableDistance) == null;
    }
}