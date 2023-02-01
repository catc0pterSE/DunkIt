using Cinemachine;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Camera;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine;
using Infrastructure.CoroutineRunner;
using Infrastructure.Factory;
using Modules.StateMachine;
using Scene;
using UI;
using UI.HUD;
using UI.HUD.Mobile;
using UnityEngine;
using Utility.Constants;

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
            IGameplayHUD gameplayHUDView = SpawnHUD();
            SceneConfig sceneConfig = GameObject.FindObjectOfType<SceneConfig>();
            Ball ball = SpawnBall();
           EnemyFacade[] enemyTeam = SpawnEnemyTeam();
            Referee referee = SpawnReferee();
            CameraFacade camera = SpawnCamera();
            PlayerFacade[] playerTeam = SpawnPlayerTeam(camera.Camera, ball, sceneConfig);
            
            GameplayLoopStateMachine gameplayLoopStateMachine =
                new GameplayLoopStateMachine(playerTeam, enemyTeam, referee, camera, gameplayHUDView, ball, sceneConfig, LoadingCurtain,
                    _coroutineRunner, _gameStateMachine);
            
            _gameStateMachine.Enter<GamePlayLoopState, GameplayLoopStateMachine>(gameplayLoopStateMachine);
        }

        private PlayerFacade[] SpawnPlayerTeam(Camera camera, Ball ball, SceneConfig sceneConfig)
        {
            PlayerFacade primaryPlayer = _gameObjectFactory.CreatePlayer();
            PlayerFacade secondaryPlayer = _gameObjectFactory.CreatePlayer();
            primaryPlayer.Initialize(secondaryPlayer, ball, camera, SpawnVirtualCamera(), sceneConfig);
            secondaryPlayer.Initialize(primaryPlayer, ball, camera, SpawnVirtualCamera(), sceneConfig);
            
            PlayerFacade[] playerTeam = new PlayerFacade[NumericConstants.PlayersInTeam];
            playerTeam[NumericConstants.PrimaryTeamMemberIndex] = primaryPlayer;
            playerTeam[NumericConstants.SecondaryTeamMemberIndex] = secondaryPlayer;
            return playerTeam;
        }

        
        private EnemyFacade[] SpawnEnemyTeam()
        {
            EnemyFacade[] enemyTeam = new EnemyFacade[NumericConstants.PlayersInTeam];

            for (int i = 0; i < enemyTeam.Length; i++)
            {
                EnemyFacade enemy = _gameObjectFactory.CreateEnemy().GetComponent<EnemyFacade>();
                enemyTeam[i] = enemy;
            }

            return enemyTeam;
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


        private IGameplayHUD SpawnHUD()  //TODO different for different platforms
        {
            MobileGameplayHUD mobileGameplayHUD = _gameObjectFactory.CreateMobileHUD();
            return mobileGameplayHUD;
        }
            


        private CinemachineVirtualCamera SpawnVirtualCamera() =>
            _gameObjectFactory.CreateCinemachineVirtualCamera();
    }
}