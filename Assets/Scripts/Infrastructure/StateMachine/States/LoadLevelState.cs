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
using Infrastructure.Mediator;
using Infrastructure.PlayerService;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;
using Scene;
using UI;
using UI.HUD;
using UI.HUD.Mobile;
using UI.HUD.StateMachine;
using UI.HUD.StateMachine.States;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

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

        public void Enter(string payload)
        {
            LoadingCurtain.Show();
            _sceneLoader.LoadScene(payload, OnLoaded);
        }

        public void Exit()
        {
            LoadingCurtain.FadeOut();
        }

        private void OnLoaded()
        {
            SceneInitials sceneInitials = GameObject.FindObjectOfType<SceneInitials>();
            Ball ball = SpawnBall();
            ball.Initialize(sceneInitials);
            Referee referee = SpawnReferee();
            referee.Initialize(ball);
            CameraFacade camera = SpawnCamera();

            PlayerFacade[] leftTeam = SpawnTeam();
            PlayerFacade[] rightTeam = SpawnTeam();

            TeamsMediator teamsMediator = new TeamsMediator
            (
                leftTeam,
                rightTeam,
                ball,
                camera,
                sceneInitials,
                _serviceContainer.Single<IInputService>(),
                _serviceContainer.Single<IPlayerService>()
            );

            leftTeam.Map(player => player.Initialize(teamsMediator, true, false, SpawnVirtualCamera()));
            rightTeam.Map(player => player.Initialize(teamsMediator, false, true, SpawnVirtualCamera()));
            _serviceContainer.Single<IPlayerService>().Set(leftTeam.First()); //TODO: move somewhere

            IGameplayHUD gameplayHUDView = SpawnHUD().Initialize(leftTeam.Union(rightTeam).ToArray(), camera.Camera, _serviceContainer.Single<IHUDStateController>());

            GameplayLoopStateMachine gameplayLoopStateMachine =
                new GameplayLoopStateMachine
                (
                    leftTeam,
                    rightTeam,
                    referee,
                    camera,
                    gameplayHUDView,
                    ball,
                    sceneInitials,
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