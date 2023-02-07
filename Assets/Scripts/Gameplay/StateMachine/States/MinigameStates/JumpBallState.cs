using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Infrastructure.Factory;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;
using UI.HUD;
using Utility.Constants;

namespace Gameplay.StateMachine.States.MinigameStates
{
    using Ball.MonoBehavior;

    public class JumpBallState : MinigameState, IParameterlessState
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly EnemyFacade[] _enemyTeam;
        private readonly Referee _referee;
        private readonly Ball _ball;
        private readonly CinemachineBrain _gameplayCamera;
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
          
            gameplayHUD
        )
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _referee = referee;
            _ball = ball;
            _gameplayCamera = gameplayCamera;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        private PlayerFacade PrimaryPlayer => _playerTeam[NumericConstants.PrimaryTeamMemberIndex];
        private PlayerFacade SecondaryPlayer => _playerTeam[NumericConstants.SecondaryTeamMemberIndex];
        private EnemyFacade PrimaryEnemy => _enemyTeam[NumericConstants.PrimaryTeamMemberIndex];
        private EnemyFacade SecondaryEnemy => _enemyTeam[NumericConstants.SecondaryTeamMemberIndex];

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
        
        protected override void InitializeMinigame()
        {
            Minigame = Services.Container.Single<IGameObjectFactory>().CreateJumpBallMinigame()
                .Initialize(_gameplayCamera, _referee, _playerTeam, _enemyTeam, _ball);
        }


        protected override void OnMiniGameWon()
        {
            _ball.SetOwner(PrimaryPlayer);
            EnterNextState();
        }

        protected override void OnMiniGameLost()
        {
            _ball.SetOwner(PrimaryEnemy);
            EnterNextState();
        }

        protected override void SetCharactersStates()
        {
            PrimaryPlayer.EnterNotControlledState();
            PrimaryEnemy.EnterNotControlledState();
            SecondaryPlayer.EnterIdleState();
            SecondaryEnemy.EnterIdleState();
        }

        private void EnterNextState() =>
            _gameplayLoopStateMachine.Enter<GameplayState>();
    }
}