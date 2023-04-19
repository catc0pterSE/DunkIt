using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Conditions
{
    public class IsSomePlayerInThrowDistance: ConditionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> PlayerToCheck;

        protected override bool OnCheck() => PlayerToCheck.value.IsInThrowDistance;
    }
}