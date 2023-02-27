using Cinemachine;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame;
using Gameplay.Minigame.JumpBall;
using Gameplay.StateMachine.States.Gameplay;
using Infrastructure.Factory;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;
using UI.HUD;
using Utility.Constants;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public class JumpBallState : MinigameState, IParameterlessState
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly PlayerFacade[] _enemyTeam;
        private readonly Referee _referee;
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly CinemachineBrain _gameplayCamera;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly IInputService _inputService;
        private readonly JumpBallMinigame _jumpBallMinigame;

        public JumpBallState
        (
            PlayerFacade[] playerTeam,
            PlayerFacade[] enemyTeam,
            Referee referee,
            Ball.MonoBehavior.Ball ball,
            CinemachineBrain gameplayCamera,
            IGameplayHUD gameplayHUD,
            GameplayLoopStateMachine gameplayLoopStateMachine,
            IGameObjectFactory gameObjectFactory,
            IInputService inputService
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
            _inputService = inputService;
            _jumpBallMinigame = gameObjectFactory.CreateJumpBallMinigame();
        }

        protected override IMinigame Minigame => _jumpBallMinigame;
        private PlayerFacade PrimaryPlayer => _playerTeam[NumericConstants.PrimaryTeamMemberIndex];
        private PlayerFacade SecondaryPlayer => _playerTeam[NumericConstants.SecondaryTeamMemberIndex];
        private PlayerFacade PrimaryEnemy => _enemyTeam[NumericConstants.PrimaryTeamMemberIndex];
        private PlayerFacade SecondaryEnemy => _enemyTeam[NumericConstants.SecondaryTeamMemberIndex];

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

        protected override void InitializeMinigame() =>
            _jumpBallMinigame.Initialize(_gameplayCamera, _referee, _playerTeam, _enemyTeam, _ball, _inputService);


        protected override void OnMiniGameWon() =>
            _ball.SetOwnerSmoothly(PrimaryPlayer, EnterGameplayState);


        protected override void OnMiniGameLost() =>
            _ball.SetOwnerSmoothly(PrimaryEnemy, EnterGameplayState);


        protected override void SetCharactersStates()
        {
            PrimaryPlayer.EnterNotControlledState();
            PrimaryEnemy.EnterNotControlledState();
            SecondaryPlayer.EnterIdleState();
            SecondaryEnemy.EnterIdleState();
        }

        private void EnterGameplayState() =>
            _gameplayLoopStateMachine.Enter<GameplayState>();
    }
}