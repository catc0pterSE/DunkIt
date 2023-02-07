using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public class BallContestState: IParameterState<(PlayerFacade, PlayerFacade)>
    {
        public void Enter((PlayerFacade, PlayerFacade) payLoad)
        {
        }

        public void Exit()
        {
          
        }
    }
}