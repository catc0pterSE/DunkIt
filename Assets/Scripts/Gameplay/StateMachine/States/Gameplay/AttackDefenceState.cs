using System.Linq;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.Transitions;
using Infrastructure.CoroutineRunner;
using Modules.StateMachine;
using Scene;
using UI.HUD;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class AttackDefenceState : StateWithTransitions, IParameterState<PlayerFacade>
    {
        private readonly PlayerFacade[] _leftTeam;
        private readonly PlayerFacade[] _rightTeam;
        private readonly IGameplayHUD _gameplayHUD;

        private PlayerFacade[] _attackingTeam;
        private PlayerFacade[] _defencingTeam;

        public AttackDefenceState
        (
            PlayerFacade[] leftTeam,
            PlayerFacade[] rightTeam,
            Ball.MonoBehavior.Ball ball,
            SceneInitials sceneInitials,
            IGameplayHUD gameplayHUD,
            GameplayLoopStateMachine gameplayLoopStateMachine,
            ICoroutineRunner coroutineRunner
        )
        {
            _leftTeam = leftTeam;
            _rightTeam = rightTeam;
            _gameplayHUD = gameplayHUD;

            Transitions = new ITransition[]
            {
                new AnyToDunkStateTransition(leftTeam.Union(rightTeam).ToArray(), gameplayLoopStateMachine),
                new AnyToThrowStateTransition(leftTeam.Union(rightTeam).ToArray(), gameplayLoopStateMachine),
                new AnyToFightForBallTransition(leftTeam.Union(rightTeam).ToArray(), gameplayLoopStateMachine, coroutineRunner, 5),
                new AnyToPassTransition(leftTeam.Union(rightTeam).ToArray(), gameplayLoopStateMachine),
            };
        }

        public void Enter(PlayerFacade ballOwner)
        {
            base.Enter();
            _gameplayHUD.Enable();
            _attackingTeam = _leftTeam.Contains(ballOwner) ? _leftTeam : _rightTeam;
            _defencingTeam = _rightTeam.Contains(ballOwner) ? _leftTeam : _rightTeam;
            SetPlayersStates(ballOwner);
        }

        public override void Exit()
        {
            base.Exit();
            _gameplayHUD.Disable();
        }

        private void SetPlayersStates(PlayerFacade ballOwner)
        {
            _attackingTeam.Map(player =>
            {
                if (player == ballOwner)
                    player.EnterAttackWithBallState();
                else
                    player.EnterAttackWithoutBallState();
            });
            
            _defencingTeam.Map(player => player.EnterDefenceState());
        }
    }
}