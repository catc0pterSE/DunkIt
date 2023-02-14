﻿using System.Linq;
using Cinemachine;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Camera;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine;
using Infrastructure.CoroutineRunner;
using Infrastructure.Factory;
using Modules.StateMachine;
using Scene.Ring;
using UI;
using UI.HUD;
using UI.HUD.Mobile;
using UnityEngine;
using Utility.Constants;
using z_Test;
using SceneConfig = Scene.SceneConfig;

namespace Infrastructure.StateMachine.States
{
    public class LoadLevelState : IParameterState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly ICoroutineRunner _coroutineRunner;

        private LoadingCurtain _loadingCurtain;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            IGameObjectFactory gameObjectFactory, ICoroutineRunner coroutineRunner)
        {
            _gameObjectFactory = gameObjectFactory;
            _coroutineRunner = coroutineRunner;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        private LoadingCurtain LoadingCurtain => _loadingCurtain ??=
            _gameObjectFactory.CreateLoadingCurtain().GetComponent<LoadingCurtain>();

        public void Enter(string name)
        {
            LoadingCurtain.Show();
            _sceneLoader.LoadScene(name, OnLoaded);
        }

        public void Exit()
        {
            LoadingCurtain.FadeOut();
        }

        private void OnLoaded()
        {
            SceneConfig sceneConfig = GameObject.FindObjectOfType<SceneConfig>();
            Ball ball = SpawnBall();
            Referee referee = SpawnReferee();
            CameraFacade camera = SpawnCamera();
            PlayerFacade[] playerTeam = SpawnPlayerTeam(camera.Camera, ball, sceneConfig, true);
            PlayerFacade[] enemyTeam = SpawnPlayerTeam(camera.Camera, ball, sceneConfig, false);
            IGameplayHUD gameplayHUDView = SpawnHUD().Initialize(playerTeam.Union(enemyTeam).ToArray(), camera.Camera);

            GameplayLoopStateMachine gameplayLoopStateMachine =
                new GameplayLoopStateMachine(playerTeam, enemyTeam, referee, camera, gameplayHUDView, ball, sceneConfig,
                    LoadingCurtain,
                    _coroutineRunner, _gameStateMachine);

            _gameStateMachine.Enter<GamePlayLoopState, GameplayLoopStateMachine>(gameplayLoopStateMachine);
        }

        private PlayerFacade[] SpawnPlayerTeam(Camera camera, Ball ball, SceneConfig sceneConfig, bool isPlayable)
        {
            PlayerFacade[] playerTeam = new PlayerFacade[NumericConstants.PlayersInTeam];

            PlayerFacade primaryPlayer = _gameObjectFactory.CreatePlayer();
            PlayerFacade secondaryPlayer = _gameObjectFactory.CreatePlayer();

            if (isPlayable == false) //TODO: remove. for test
            {
                primaryPlayer.GetComponentInChildren<MeshRenderer>().material =
                    ball.GetComponentInChildren<MeshRenderer>().material;
                secondaryPlayer.GetComponentInChildren<MeshRenderer>().material =
                    ball.GetComponentInChildren<MeshRenderer>().material;
            }

            Ring enemyRing = isPlayable ? sceneConfig.EnemyRing : sceneConfig.PlayerRing;

            playerTeam[NumericConstants.PrimaryTeamMemberIndex] = primaryPlayer.Initialize(isPlayable, secondaryPlayer,
                ball, camera, SpawnVirtualCamera(), enemyRing);
            playerTeam[NumericConstants.SecondaryTeamMemberIndex] = secondaryPlayer.Initialize(isPlayable,
                primaryPlayer, ball, camera, SpawnVirtualCamera(), enemyRing);

            return playerTeam;
        }

        private Referee SpawnReferee()
        {
            Referee referee = _gameObjectFactory.CreateReferee();
            return referee;
        }

        private CameraFacade SpawnCamera() =>
            _gameObjectFactory.CreateCamera();


        private Ball SpawnBall() =>
            _gameObjectFactory.CreateBall();


        private IGameplayHUD SpawnHUD() //TODO different for different platforms
        {
            MobileGameplayHUD mobileGameplayHUD = _gameObjectFactory.CreateMobileHUD();
            return mobileGameplayHUD;
        }

        private CinemachineVirtualCamera SpawnVirtualCamera() =>
            _gameObjectFactory.CreateCinemachineVirtualCamera();
    }
}