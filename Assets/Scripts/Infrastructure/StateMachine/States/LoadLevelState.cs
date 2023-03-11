using System.Linq;
using Cinemachine;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Camera;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine;
using Infrastructure.CoroutineRunner;
using Infrastructure.Factory;
using Infrastructure.Input;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;
using Scene.Ring;
using UI;
using UI.HUD;
using UI.HUD.Mobile;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;
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
        private readonly Services _serviceContainer;
        private readonly IInputService _inputService;

        private LoadingCurtain _loadingCurtain;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            ICoroutineRunner coroutineRunner, Services serviceContainer)
        {
            _coroutineRunner = coroutineRunner;
            _serviceContainer = serviceContainer;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameObjectFactory = serviceContainer.Single<IGameObjectFactory>();
            _inputService = serviceContainer.Single<IInputService>();
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

            PlayerFacade[] playerTeam = SpawnTeam();
            PlayerFacade[] enemyTeam = SpawnTeam();

            InitializeTeam(playerTeam, enemyTeam, ball, sceneConfig, true, camera.Camera);
            InitializeTeam(enemyTeam, playerTeam, ball, sceneConfig, false, camera.Camera);

            IGameplayHUD gameplayHUDView = SpawnHUD().Initialize(playerTeam.Union(enemyTeam).ToArray(), camera.Camera);

            GameplayLoopStateMachine gameplayLoopStateMachine =
                new GameplayLoopStateMachine
                (
                    playerTeam,
                    enemyTeam,
                    referee,
                    camera,
                    gameplayHUDView,
                    ball,
                    sceneConfig,
                    LoadingCurtain,
                    _coroutineRunner,
                    _gameObjectFactory,
                    _inputService,
                    _gameStateMachine
                );

            _gameStateMachine.Enter<GamePlayLoopState, GameplayLoopStateMachine>(gameplayLoopStateMachine);
        }

        private PlayerFacade[] SpawnTeam()
        {
            PlayerFacade[] team = new PlayerFacade[NumericConstants.PlayersInTeam];

            for (int i = 0; i < team.Length; i++)
                team[i] = _gameObjectFactory.CreatePlayer();

            return team;
        }

        private void InitializeTeam(PlayerFacade[] team, PlayerFacade[] oppositeTeam, Ball ball,
            SceneConfig sceneConfig, bool isPlayable, Camera camera)
        {
            if (isPlayable == false) //TODO: TEST - delete
                team.Map(player => player.GetComponentInChildren<MeshRenderer>().material =
                    ball.GetComponentInChildren<MeshRenderer>().material);

            PlayerFacade primaryPlayer = team[NumericConstants.PrimaryTeamMemberIndex];
            PlayerFacade secondaryPlayer = team[NumericConstants.SecondaryTeamMemberIndex];

            primaryPlayer.Initialize(isPlayable, secondaryPlayer, ball, oppositeTeam, camera, SpawnVirtualCamera(),
                sceneConfig,
                _inputService);
            secondaryPlayer.Initialize(isPlayable, primaryPlayer, ball, oppositeTeam, camera, SpawnVirtualCamera(),
                sceneConfig,
                _inputService);
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
            return SpawnMobileHUD();
        }

        private IGameplayHUD SpawnMobileHUD()
        {
            MobileGameplayHUD mobileGameplayHUD = _gameObjectFactory.CreateMobileHUD();
            mobileGameplayHUD.SetUiInputController(_serviceContainer.Single<IUIInputController>());
            return mobileGameplayHUD;
        }

        private CinemachineVirtualCamera SpawnVirtualCamera() =>
            _gameObjectFactory.CreateCinemachineVirtualCamera();
    }
}