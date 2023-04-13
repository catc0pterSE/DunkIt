using System.Linq;
using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Modules.StateMachine;
using Scene;
using Scene.Ring;

namespace Gameplay.StateMachine.Transitions
{
    using Ball.MonoBehavior;
    
    public class AnyToDropBallTransition : ITransition
    {
        private readonly Ball _ball;
        private readonly PlayerFacade[] _leftTeam;
        private readonly PlayerFacade[] _rightTeam;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public AnyToDropBallTransition(Ball ball, PlayerFacade[] leftTeam, PlayerFacade[] rightTeam, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _ball = ball;
            _leftTeam = leftTeam;
            _rightTeam = rightTeam;
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
            _ball.Lost += OnBallLost;

        private void UnsubscribeFromBall() =>
            _ball.Lost -= OnBallLost;

        private void OnBallLost(CharacterFacade lastOwner)
        {
            if (_leftTeam.Contains(lastOwner))
                EnterRightTeamDropsBall();
            
            if (_rightTeam.Contains(lastOwner))
                EnterLeftTeamDropsBall();
        }
        
        private void EnterLeftTeamDropsBall() =>
            _gameplayLoopStateMachine.Enter<DropBallState, PlayerFacade>(_leftTeam.First());
        
        private void EnterRightTeamDropsBall() =>
            _gameplayLoopStateMachine.Enter<DropBallState, PlayerFacade>(_rightTeam.First());
    }
}