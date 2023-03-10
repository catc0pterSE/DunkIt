using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Infrastructure.Factory;
using Infrastructure.ServiceManagement;
using UI.HUD;
using UI.HUD.Mobile;
using Utility.Constants;

namespace Gameplay.StateMachine.States.MinigameStates
{
    using Ball.MonoBehavior;

    public class JumpBallState : MinigameState
    {
        private readonly PlayerFacade _player;
        private readonly EnemyFacade _enemy;
        private readonly Referee _referee;
        private readonly Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public JumpBallState
        (
            PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam,
            Referee referee,
            Ball ball,
            CinemachineBrain gameplayCamera,
            IGameplayHUD gameplayHUD,
            GameplayLoopStateMachine gameplayLoopStateMachine
        ) : base
        (
            playerTeam,
            enemyTeam,
            gameplayHUD,
            Services.Container.Single<IGameObjectFactory>().CreateJumpBallMinigame()
                .Initialize(gameplayCamera, referee, playerTeam, enemyTeam, ball)
        )
        {
            _player = playerTeam[NumericConstants.PrimaryTeamMemberIndex];
            _enemy = enemyTeam[NumericConstants.PrimaryTeamMemberIndex];
            _referee = referee;
            _ball = ball;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public override void Enter()
        {
            base.Enter();
            _referee.Enable();
            _ball.SetOwner(_referee);
        }

        public override void Exit()
        {
            base.Exit();
            _referee.Disable();
        }

        protected override void OnMiniGameWon()
        {
            _ball.SetOwner(_player);
            EnterNextState();
        }

        protected override void OnMiniGameLost()
        {
            _ball.SetOwner(_enemy);
            EnterNextState();
        }

        private void EnterNextState() =>
            _gameplayLoopStateMachine.Enter<GameplayState>();
    }
}