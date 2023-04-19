using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.New.Conditions
{
    public class SomePlayerCanPass : ConditionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> PlayerToCheck;

        protected override bool OnCheck() => PlayerToCheck.value.CanPass;

    }
}