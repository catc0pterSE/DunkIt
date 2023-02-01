using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.MinigameStates;
using Modules.StateMachine;

namespace Gameplay.StateMachine.Tranzitions
{
    using Ball.MonoBehavior;
    using Character;

    public class AnyToBallContestStateTransition : ITransition
    {
        private readonly Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private BasketballPlayer _currentBallOwner;

        public AnyToBallContestStateTransition(Ball ball, GameplayLoopStateMachine gameplayLoopStateMachine)
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

        private void OnBallOwnerChanged(Character newOwner)
        {
            if (_currentBallOwner != null)
                UnsubscribeFromCurrentBallOwner();

            if (newOwner is not BasketballPlayer basketballPlayer)
                return;

            _currentBallOwner = basketballPlayer;
            SubscribeOnCurrentBallOwner();
        }

        private void SubscribeOnCurrentBallOwner() =>
            _currentBallOwner.BallContestStarted += MoveToBallContestState;

        private void UnsubscribeFromCurrentBallOwner() =>
            _currentBallOwner.BallContestStarted -= MoveToBallContestState;

        private void MoveToBallContestState(PlayerFacade player, EnemyFacade enemy)
        {
            _gameplayLoopStateMachine.Enter<BallContestState, (PlayerFacade, EnemyFacade)>((player, enemy));
        }
    }
}