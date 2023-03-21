using Gameplay.Character.Player.MonoBehaviour.Movement;
using NodeCanvas.Framework;
using UnityEngine;

namespace NC_Custom_Tasks.Actions
{
    public class RotateTo: ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerMover> HostMover;
        public BBParameter<Transform> Target;

        protected override void OnExecute()
        {
            HostMover.value.RotateTo(Target.value.position);
            EndAction(true);
        }
    }
}