using System.Linq;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.Transitions;
using Modules.StateMachine;
using Scene;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class BallChasingState : StateWithTransitions, IParameterlessState
    {
        private readonly PlayerFacade[] _players;

        public BallChasingState(PlayerFacade[] leftTeam, PlayerFacade[] rightTeam, Ball.MonoBehavior.Ball ball,
            SceneInitials sceneInitials, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _players = leftTeam.Union(rightTeam).ToArray();

            Transitions = new ITransition[]
            {
                new AnyToAttackDefenceStateTransition(ball, gameplayLoopStateMachine),
                new AnyToDropBallTransition(ball, leftTeam, rightTeam, sceneInitials, gameplayLoopStateMachine)
            };
        }

        public override void Enter()
        {
            base.Enter();
            SetPlayersStates();
        }

        private void SetPlayersStates() =>
            _players.Map(player => player.EnterBallChasingState());
    }
}