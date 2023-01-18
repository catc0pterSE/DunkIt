using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.HUD;
using Gameplay.StateMachine.States.MiniGameStates;
using Infrastructure.Factory;
using Infrastructure.ServiceManagement;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    using Ball.MonoBehavior;
    public class StartCutsceneState : CutsceneState
    {
        private readonly Referee _referee;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public StartCutsceneState
        (PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam,
            Referee referee,
            Ball ball,
            CinemachineBrain camera,
            GameplayHUD gameplayHUD,
            GameplayLoopStateMachine gameplayLoopStateMachine) : base
        (
            playerTeam,
            enemyTeam,
            gameplayHUD,
            Services.Container.Single<IGameObjectFactory>().CreateStartCutscene().Initialize(camera, playerTeam, enemyTeam, referee, ball)
        )
        {
            _referee = referee;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        protected override void EnterNextState()
        {
           _gameplayLoopStateMachine.Enter<RefereeBallState>();
        }
    }
}