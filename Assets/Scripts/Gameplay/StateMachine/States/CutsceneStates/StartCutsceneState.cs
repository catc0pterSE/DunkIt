using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.MiniGameStates;
using Infrastructure.Factory;
using Infrastructure.ServiceManagement;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    public class StartCutsceneState : CutsceneState
    {
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public StartCutsceneState
        (
            PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam,
            Referee referee,
            Ball.MonoBehavior.Ball ball,
            CinemachineBrain camera,
            GameplayLoopStateMachine gameplayLoopStateMachine
        ) : base
        (
            playerTeam,
            enemyTeam,
            Services.Container.Single<IGameObjectFactory>().CreateStartCutscene().Initialize(camera, playerTeam, enemyTeam, referee, ball)
        )
        {
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        protected override void EnterNextState()
        {
           _gameplayLoopStateMachine.Enter<RefereeBallState>();
        }
    }
}