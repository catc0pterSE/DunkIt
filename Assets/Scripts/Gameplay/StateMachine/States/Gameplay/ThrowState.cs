using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.Transitions;
using Modules.StateMachine;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class ThrowState : StateWithTransitions, IParameterState<PlayerFacade>
    {
      private PlayerFacade _throwingPlayer;

        public ThrowState(
            Ball.MonoBehavior.Ball ball,
            GameplayLoopStateMachine gameplayLoopStateMachine
        )
        {
            Transitions = new ITransition[]
            {
                new AnyToBallChasingStateTransition(ball, gameplayLoopStateMachine)
            };
        }

        public void Enter(PlayerFacade throwingPlayer)
        { 
            base.Enter();
            throwingPlayer.EnterThrowState();
        }
    }
}