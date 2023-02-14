using System;
using System.Collections.Generic;
using Gameplay.Camera;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.CutsceneStates;
using Gameplay.StateMachine.States.Gameplay;
using Gameplay.StateMachine.States.MinigameStates;
using Infrastructure.CoroutineRunner;
using Infrastructure.StateMachine;
using Modules.StateMachine;
using UI;
using UI.HUD;
using SceneConfig = Scene.SceneConfig;

namespace Gameplay.StateMachine
{
    using Ball.MonoBehavior;
    public class GameplayLoopStateMachine : Modules.StateMachine.StateMachine
    {
        public GameplayLoopStateMachine(PlayerFacade[] playerTeam,
            PlayerFacade[] enemyTeam,
            Referee referee,
            CameraFacade camera,
            IGameplayHUD gameplayHUD,
            Ball ball,
            SceneConfig sceneConfig,
            LoadingCurtain loadingCurtain,
            ICoroutineRunner coroutineRunner,
            GameStateMachine gameStateMachine)
        {
            States = new Dictionary<Type, IState>
            {
                [typeof(StartCutsceneState)] = new StartCutsceneState(playerTeam, enemyTeam, referee, camera.CinemachineBrain, ball, gameplayHUD, this),
                [typeof(JumpBallState)] = new JumpBallState(playerTeam, enemyTeam, referee, ball, camera.CinemachineBrain, gameplayHUD, this),
                [typeof(GameplayState)] = new GameplayState(playerTeam, enemyTeam, ball, sceneConfig, gameplayHUD, this, loadingCurtain, coroutineRunner),
                [typeof(PassState)] = new PassState(playerTeam, enemyTeam, this),
                [typeof(DunkState)] = new DunkState(playerTeam, enemyTeam, ball, sceneConfig, this),
                [typeof(ThrowState)] = new ThrowState(gameplayHUD, sceneConfig, this, enemyTeam, ball, loadingCurtain, coroutineRunner),
                [typeof(BallContestState)] = new BallContestState(ball, gameplayHUD),
                [typeof(CelebrateCutsceneState)] = new CelebrateCutsceneState(),
                [typeof(UpsetCutsceneState)] = new UpsetCutsceneState()
            };
        }

        public void Run() =>
            Enter<StartCutsceneState>();
    }
}