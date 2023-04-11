using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.Transitions;
using Modules.StateMachine;
using UI.HUD;

namespace Gameplay.StateMachine.States.Gameplay
{
    using Ball.MonoBehavior;
    public class PassState : StateWithTransitions, IParameterState<PlayerFacade, PlayerFacade>
    {
        private readonly PlayerFacade[] _players;
        private readonly IGameplayHUD _gameplayHUD;

        public PassState(Ball ball, IGameplayHUD gameplayHUD, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _gameplayHUD = gameplayHUD;
            Transitions = new ITransition[]
            {
                new AnyToBallChasingStateTransition(ball, gameplayLoopStateMachine)
            };
        }

        public void Enter(PlayerFacade passingPlayer, PlayerFacade passTarget)
        {
            base.Enter();
            SetPlayersStates(passingPlayer, passTarget);
            _gameplayHUD.Disable();
        }

        private void SetPlayersStates(PlayerFacade passingPlayer, PlayerFacade passTarget) =>
            passingPlayer.EnterPassState(passTarget);
    }
}