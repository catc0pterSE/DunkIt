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
        private readonly Ring _leftRing;
        private readonly Ring _ringRing;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public AnyToDropBallTransition(Ball ball, PlayerFacade[] leftTeam, PlayerFacade[] rightTeam, SceneInitials sceneInitials, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _ball = ball;
            _leftTeam = leftTeam;
            _rightTeam = rightTeam;
            _leftRing = sceneInitials.LeftRing;
            _ringRing = sceneInitials.RightRing;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable()
        {
            SubscribeOnRings();
            SubscribeOnBall();
        }

        public void Disable()
        {
            UnsubscribeFromRings();
            UnsubscribeFromBall();
        }

        private void SubscribeOnBall() =>
            _ball.Lost += OnBallLost;

        private void UnsubscribeFromBall() =>
            _ball.Lost -= OnBallLost;

        private void SubscribeOnRings()
        {
            _leftRing.Goal += OnLeftRingGoalScored;
            _ringRing.Goal += OnRightRingGoalScored;
        }

        private void UnsubscribeFromRings()
        {
            _leftRing.Goal -= OnLeftRingGoalScored;
            _ringRing.Goal -= OnRightRingGoalScored;
        }

        private void OnLeftRingGoalScored() =>
            EnterLeftTeamDropsBall();

        private void OnRightRingGoalScored() =>
            EnterRightTeamDropsBall();

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