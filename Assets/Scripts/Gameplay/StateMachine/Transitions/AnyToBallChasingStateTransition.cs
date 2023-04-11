using Gameplay.Character;
using Gameplay.StateMachine.States.Gameplay;
using Modules.StateMachine;

namespace Gameplay.StateMachine.Transitions
{
    using Ball.MonoBehavior;

    public class AnyToBallChasingStateTransition : ITransition
    {
        private readonly Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public AnyToBallChasingStateTransition(Ball ball, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _ball = ball;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable() =>
            SubscribeOnBall();

        public void Disable() =>
            UnsubscribeFromBall();

        private void SubscribeOnBall() =>
            _ball.Free += OnBallOwnerChanged;

        private void UnsubscribeFromBall() =>
            _ball.Free -= OnBallOwnerChanged;

        private void OnBallOwnerChanged() =>
            _gameplayLoopStateMachine.Enter<BallChasingState>();
    }
}