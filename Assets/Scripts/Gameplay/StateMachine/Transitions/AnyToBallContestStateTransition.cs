using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.MinigameStates;
using Modules.StateMachine;

namespace Gameplay.StateMachine.Transitions
{
    public class AnyToBallContestStateTransition : ITransition
    {
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private PlayerFacade _currentBallOwner;

        public AnyToBallContestStateTransition(Ball.MonoBehavior.Ball ball, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _ball = ball;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable()
        {
            SubscribeOnBall();
            OnBallOwnerChanged(_ball.Owner);
        }

        public void Disable()
        {
            UnsubscribeFromBall();
            UnsubscribeFromCurrentBallOwner();
        }

        private void SubscribeOnBall() =>
            _ball.OwnerChanged += OnBallOwnerChanged;

        private void UnsubscribeFromBall() =>
            _ball.OwnerChanged -= OnBallOwnerChanged;

        private void OnBallOwnerChanged(CharacterFacade newOwner)
        {
            if (_currentBallOwner != null)
                UnsubscribeFromCurrentBallOwner();

            if (newOwner is not PlayerFacade basketballPlayer)
                return;

            _currentBallOwner = basketballPlayer;
            SubscribeOnCurrentBallOwner();
        }

        private void SubscribeOnCurrentBallOwner() =>
            _currentBallOwner.BallContestStarted += MoveToBallContestState;

        private void UnsubscribeFromCurrentBallOwner() =>
            _currentBallOwner.BallContestStarted -= MoveToBallContestState;

        private void MoveToBallContestState(PlayerFacade player, PlayerFacade enemy)
        {
            _gameplayLoopStateMachine.Enter<BallContestState, (PlayerFacade, PlayerFacade)>((player, enemy));
        }
    }
}