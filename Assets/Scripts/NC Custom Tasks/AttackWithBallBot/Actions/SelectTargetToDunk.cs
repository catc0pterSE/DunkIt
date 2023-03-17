using Gameplay.Character.Player.MonoBehaviour.Distance;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.AttackWithBallBot.Actions
{
    public class SelectTargetToDunk : SelectTarget
    {
        [BlackboardOnly] public BBParameter<TargetTracker> TargetTracker;
        public float Delta;
        protected override void OnExecute()
        {
            Distance = TargetTracker.value.MaxDunkDistance - Delta;
            base.OnExecute();
        }
    }
}