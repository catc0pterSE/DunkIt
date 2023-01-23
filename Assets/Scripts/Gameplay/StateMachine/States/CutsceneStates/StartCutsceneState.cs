using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.HUD;
using Gameplay.StateMachine.States.MinigameStates;
using Infrastructure.Factory;
using Infrastructure.ServiceManagement;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    using Ball.MonoBehavior;
    public class StartCutsceneState : CutsceneState
    {
        private readonly Referee _referee;
        private readonly Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public StartCutsceneState
        (PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam,
            Referee referee,
            CinemachineBrain camera,
            Ball ball,
            GameplayHUD gameplayHUD,
            GameplayLoopStateMachine gameplayLoopStateMachine) : base
        (
            playerTeam,
            enemyTeam,
            gameplayHUD,
            Services.Container.Single<IGameObjectFactory>().CreateStartCutscene().Initialize(camera, playerTeam, enemyTeam, referee)
        )
        {
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

        protected override void EnterNextState()
        {
           _gameplayLoopStateMachine.Enter<JumpBallState>();
        }
    }
}