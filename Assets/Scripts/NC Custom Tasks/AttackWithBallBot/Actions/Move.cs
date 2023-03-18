using Gameplay.Character.Player.MonoBehaviour.Movement;
using NodeCanvas.Framework;
using UnityEngine;

namespace NC_Custom_Tasks.AttackWithBallBot.Actions
{
    public class Move : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerMover> Mover;
        [BlackboardOnly] public BBParameter<Vector3> Direction;

        protected override void OnExecute()
        {
            Debug.DrawRay(Mover.value.transform.position, Direction.value*10, Color.green);
            Mover.value.MoveLookingStraight(Direction.value);
            EndAction(true);
        }
            
    }
}