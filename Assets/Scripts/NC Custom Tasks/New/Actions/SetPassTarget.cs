using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.TargetTracking;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Actions
{
    public class SetPassTarget: ActionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> PassTarget;
        [BlackboardOnly] public BBParameter<TargetTracker> TargetTracker;

        protected override void OnExecute()
        {
            PassTarget.value = TargetTracker.value.PassTarget;
            EndAction(true);
        }
    }
}