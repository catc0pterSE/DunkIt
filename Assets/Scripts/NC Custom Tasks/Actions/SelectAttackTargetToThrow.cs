using Gameplay.Character.Player.MonoBehaviour.TargetTracking;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Actions
{
    public class SelectAttackTargetToThrow : SelectAttackTarget
    {
        [BlackboardOnly] public BBParameter<TargetTracker> TargetTracker;
        public float Delta;

        protected override void OnExecute()
        {
            Distance = TargetTracker.value.MaxThrowDistance - Delta;
            base.OnExecute();
        }
    }
}