using Gameplay.Character.Player.MonoBehaviour.Distance;
using NodeCanvas.Framework;
using Scene.Ring;

namespace NC_Custom_Tasks.AttackWithBallBot.Actions
{
    public class SelectTargetToThrow : SelectTarget
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