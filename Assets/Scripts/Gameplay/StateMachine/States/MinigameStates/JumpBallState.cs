using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.HUD;
using Gameplay.StateMachine.States.Gameplay;
using Infrastructure.Factory;
using Infrastructure.ServiceManagement;
using Utility.Constants;

namespace Gameplay.StateMachine.States.MinigameStates
{
    using Ball.MonoBehavior;

    public class JumpBallState : MinigameState
    {
        private readonly PlayerFacade _player;
        private readonly EnemyFacade _enemy;
        private readonly Referee _referee;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public JumpBallState
        (
            PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam,
            Referee referee,
            Ball ball,
            CinemachineBrain gameplayCamera,
            GameplayHUD gameplayHUD,
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
            _player = playerTeam[NumericConstants.PrimaryPlayerIndex];
            _enemy = enemyTeam[NumericConstants.PrimaryPlayerIndex];
            _referee = referee;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public override void Enter()
        {
            base.Enter();
            _referee.Enable();
            _referee.TakeBall();
        }

        public override void Exit()
        {
            base.Exit();
            _referee.Disable();
        }

        protected override void OnMiniGameWon()
        {
            _player.TakeBall();
            EnterNextState();
        }

        protected override void OnMiniGameLost()
        {
            _enemy.TakeBall();
            EnterNextState();
        }

        private void EnterNextState() =>
            _gameplayLoopStateMachine.Enter<GameplayState>();
    }
}