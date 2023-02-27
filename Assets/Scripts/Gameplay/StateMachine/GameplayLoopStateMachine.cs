﻿using System;
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
using Infrastructure.ServiceManagement;
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
                [typeof(GameplayState)] = new GameplayState(playerTeam, enemyTeam, ball, sceneConfig, gameplayHUD, this, loadingCurtain, coroutineRunner, inputService),
                [typeof(PassState)] = new PassState(playerTeam, enemyTeam, this),
                [typeof(DunkState)] = new DunkState(playerTeam, enemyTeam, ball, sceneConfig, this),
                [typeof(ThrowState)] = new ThrowState(gameplayHUD, sceneConfig, this, enemyTeam, ball, loadingCurtain, coroutineRunner, gameObjectFactory),
                [typeof(FightForBallState)] = new FightForBallState(playerTeam, enemyTeam, ball, gameplayHUD, this, gameObjectFactory),
                [typeof(CelebrateCutsceneState)] = new CelebrateCutsceneState(),
                [typeof(UpsetCutsceneState)] = new UpsetCutsceneState()
            };
        }

        public void Run() =>
            Enter<StartCutsceneState>();
    }
}