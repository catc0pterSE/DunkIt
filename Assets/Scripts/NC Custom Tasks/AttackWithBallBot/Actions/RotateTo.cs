using Gameplay.Character.Player.MonoBehaviour.Movement;
using NodeCanvas.Framework;
using UnityEngine;

namespace NC_Custom_Tasks.AttackWithBallBot.Actions
{
    public class RotateTo : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerMover> PlayerMover;
        [BlackboardOnly] public BBParameter<Transform> Target;

        protected override void OnExecute() =>
            PlayerMover.value.RotateTo(Target.value.position, () => EndAction(true));
    }
}