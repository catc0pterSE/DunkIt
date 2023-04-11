using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Modules.StateMachine;

namespace Gameplay.StateMachine.Transitions
{
    public class AnyToAttackDefenceStateTransition : ITransition
    {
       private readonly Ball.MonoBehavior.Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public AnyToAttackDefenceStateTransition(Ball.MonoBehavior.Ball ball, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _ball = ball;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable() =>
            SubscribeOnBall();

        public void Disable() =>
            UnsubscribeFromBall();

        private void SubscribeOnBall() =>
            _ball.OwnerChanged += OnBallOwnerChanged;

        private void UnsubscribeFromBall() =>
            _ball.OwnerChanged -= OnBallOwnerChanged;

        private void OnBallOwnerChanged(CharacterFacade newOwner)
        {
            if (newOwner is not PlayerFacade player)
                return;
            
            _gameplayLoopStateMachine.Enter<AttackDefenceState, PlayerFacade>(player);
        }
    }
}