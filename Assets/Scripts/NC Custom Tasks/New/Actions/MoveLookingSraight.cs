using Gameplay.Character.Player.MonoBehaviour.Movement;
using NodeCanvas.Framework;
using UnityEngine;

namespace NC_Custom_Tasks.Actions
{
    public class MoveLookingSraight : ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerMover> Mover;
        [BlackboardOnly] public BBParameter<Vector3> Direction;

        protected override void OnExecute()
        {
            Mover.value.Move(Direction.value);
            Mover.value.Rotate(Direction.value);
            EndAction(true);
        }
    }
}