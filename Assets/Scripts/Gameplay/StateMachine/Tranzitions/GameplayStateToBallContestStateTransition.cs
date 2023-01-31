using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.MinigameStates;
using Modules.StateMachine;

namespace Gameplay.StateMachine.Tranzitions
{
    using Ball.MonoBehavior;
    using Character;

    public class GameplayStateToBallContestStateTransition : ITransition
    {
        private readonly Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private BasketballPlayer _currentBallOwner;

        public GameplayStateToBallContestStateTransition(Ball ball, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _ball = ball;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable()
        {
            SubscribeOnBall();
        }

        public void Disable()
        {
            UnsubscribeFromBall();
        }

        private void SubscribeOnBall() =>
            _ball.OwnerChanged+=OnBallOwnerChanged;

        private void UnsubscribeFromBall() =>
            _ball.OwnerChanged -= OnBallOwnerChanged;

        private void OnBallOwnerChanged(Character newOwner)
        {
            if (newOwner is not BasketballPlayer basketballPlayer)
                return;

            if (_currentBallOwner != null)
                UnsubscribeOnCurrentBallOwner();

            _currentBallOwner = basketballPlayer;
            SubscribeOnCurrentBallOwner();
        }

        private void SubscribeOnCurrentBallOwner() =>
            _currentBallOwner.BallContestStarted += MoveToBallContestState;

        private void UnsubscribeOnCurrentBallOwner() =>
            _currentBallOwner.BallContestStarted -= MoveToBallContestState;

        private void MoveToBallContestState(PlayerFacade player, EnemyFacade enemy)
        {
            _gameplayLoopStateMachine.Enter<BallContestState, (PlayerFacade, EnemyFacade)>((player, enemy));
        }
    }
}