using Gameplay.Camera.MonoBehaviour;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Cutscene;
using Gameplay.StateMachine.States.MiniGameStates;
using Infrastructure.CoroutineRunner;
using Scene;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    using Ball.MonoBehavior;

    public class StartCutsceneState : CutsceneState
    {
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public StartCutsceneState
        (
            PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam,
            Referee referee,
            Ball ball,
            CameraFacade camera,
            SceneConfig sceneConfig,
            ICoroutineRunner coroutineRunner,
            GameplayLoopStateMachine gameplayLoopStateMachine
        ) : base
        (
            playerTeam,
            enemyTeam,
            camera,
            new StartCutscene(playerTeam, enemyTeam, ball, referee, camera, sceneConfig.StartCutsceneConfig,coroutineRunner))
        {
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }


        public override void Exit()
        {
        }

        protected override void EnterNextState()
        {
           _gameplayLoopStateMachine.Enter<RefereeBallState>();
        }
    }
}