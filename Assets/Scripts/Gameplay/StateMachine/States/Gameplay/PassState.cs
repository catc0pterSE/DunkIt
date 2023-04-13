using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.Transitions;
using Modules.StateMachine;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class PassState : StateWithTransitions, IParameterState<PlayerFacade, PlayerFacade>
    {
        private readonly PlayerFacade[] _players;

        public PassState(Ball.MonoBehavior.Ball ball, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            Transitions = new ITransition[]
            {
                new AnyToBallChasingStateTransition(ball, gameplayLoopStateMachine)
            };
        }

        public void Enter(PlayerFacade passingPlayer, PlayerFacade passTarget)
        {
            base.Enter();
            SetPlayersStates(passingPlayer, passTarget);
        }

        private void SetPlayersStates(PlayerFacade passingPlayer, PlayerFacade passTarget) =>
            passingPlayer.EnterPassState(passTarget);
    }
}