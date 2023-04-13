using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.Transitions;
using Infrastructure.CoroutineRunner;
using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class ThrowState : StateWithTransitions, IParameterState<PlayerFacade>
    {
      private PlayerFacade _throwingPlayer;

        public ThrowState(PlayerFacade[] players,
            Ball.MonoBehavior.Ball ball,
            GameplayLoopStateMachine gameplayLoopStateMachine,
            ICoroutineRunner coroutineRunner
        )
        {
            Transitions = new ITransition[]
            {
                new AnyToBallChasingStateTransition(ball, gameplayLoopStateMachine),
                new AnyToFightForBallTransition(players, gameplayLoopStateMachine, coroutineRunner, 2)
            };
        }

        public void Enter(PlayerFacade throwingPlayer)
        { 
            base.Enter();
            throwingPlayer.EnterThrowState();
        }
    }
}