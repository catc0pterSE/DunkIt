using System;
using System.Collections.Generic;
using Gameplay.Camera;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.CutsceneStates;
using Gameplay.StateMachine.States.Gameplay;
using Gameplay.StateMachine.States.MinigameStates;
using Infrastructure.CoroutineRunner;
using Infrastructure.Factory;
using Infrastructure.Input.InputService;
using Infrastructure.StateMachine;
using Modules.StateMachine;
using Scene;
using UI;
using UI.HUD;

namespace Gameplay.StateMachine
{
    public class GameplayLoopStateMachine : Modules.StateMachine.StateMachine
    {
        public GameplayLoopStateMachine(PlayerFacade[] playerTeam,
            PlayerFacade[] enemyTeam,
            Referee referee,
            CameraFacade camera,
            IGameplayHUD gameplayHUD,
            Ball.MonoBehavior.Ball ball,
            SceneConfig sceneConfig,
            LoadingCurtain loadingCurtain,
            ICoroutineRunner coroutineRunner,
            IGameObjectFactory gameObjectFactory,
            IInputService inputService,
            GameStateMachine gameStateMachine
        )
        {
            States = new Dictionary<Type, IState>
            {
                [typeof(StartCutsceneState)] = new StartCutsceneState(playerTeam, enemyTeam, referee,
                    camera.CinemachineBrain, ball, gameplayHUD, this, gameObjectFactory, inputService),
                [typeof(JumpBallState)] = new JumpBallState(playerTeam, enemyTeam, referee, ball, camera.CinemachineBrain, gameplayHUD, this, gameObjectFactory, inputService),
                [typeof(GameplayState)] = new GameplayState(playerTeam, enemyTeam, ball, sceneConfig, gameplayHUD, this, loadingCurtain, coroutineRunner),
                [typeof(PassState)] = new PassState(playerTeam, enemyTeam, this),
                [typeof(DunkState)] = new DunkState(playerTeam, enemyTeam, ball,  this),
                [typeof(ThrowState)] = new ThrowState(gameplayHUD, sceneConfig, this, enemyTeam, enemyTeam, ball, loadingCurtain, coroutineRunner, gameObjectFactory),
                [typeof(FightForBallState)] = new FightForBallState(playerTeam, enemyTeam, ball, gameplayHUD, this, gameObjectFactory),
                [typeof(CelebrateCutsceneState)] = new CelebrateCutsceneState(this),
                [typeof(UpsetCutsceneState)] = new UpsetCutsceneState(this)
            };
        }

        public void Run() =>
            Enter<StartCutsceneState>();
    }
}