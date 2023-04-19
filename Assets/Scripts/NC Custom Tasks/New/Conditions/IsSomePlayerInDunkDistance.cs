using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Conditions
{
    public class IsSomePlayerInDunkDistance : ConditionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> PlayerToCheck;

        protected override bool OnCheck() =>
            PlayerToCheck.value.IsInDunkDistance;
    }
}