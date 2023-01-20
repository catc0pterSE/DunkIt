using Cinemachine;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.HUD;
using Gameplay.StateMachine;
using Infrastructure.CoroutineRunner;
using Infrastructure.Factory;
using Modules.StateMachine;
using Scene;
using UI;
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
            LoadingCurtain.Hide();
        }

        private void OnLoaded()
        {
            GameplayHUD gameplayHUD = SpawnHUD();
            SceneConfig sceneConfig = GameObject.FindObjectOfType<SceneConfig>();
            CinemachineBrain camera = SpawnCamera();
            Ball ball = SpawnBall();
            PlayerFacade[] playerTeam = SpawnPlayerTeam(camera.transform, ball, sceneConfig);
            EnemyFacade[] enemyTeam = SpawnEnemyTeam(ball);
            Referee referee = SpawnReferee(ball);

            GameplayLoopStateMachine gameplayLoopStateMachine =
                new GameplayLoopStateMachine(playerTeam, enemyTeam, referee, camera, gameplayHUD, ball, sceneConfig,
                    _coroutineRunner, _gameStateMachine);

            _gameStateMachine.Enter<GamePlayLoopState, GameplayLoopStateMachine>(gameplayLoopStateMachine);
        }

        private PlayerFacade[] SpawnPlayerTeam(Transform camera, Ball ball, SceneConfig sceneConfig)
        {
            PlayerFacade[] playerTeam = new PlayerFacade[NumericConstants.PlayersInTeam];

            for (int i = 0; i < playerTeam.Length; i++)
            {
                PlayerFacade player = _gameObjectFactory.CreatePlayer().GetComponent<PlayerFacade>();
                CinemachineVirtualCamera virtualCamera = SpawnVirtualCamera();
                player.Initialize(ball, camera, virtualCamera, sceneConfig);

                playerTeam[i] = player;
            }

            return playerTeam;
        }

        private EnemyFacade[] SpawnEnemyTeam(Ball ball)
        {
            EnemyFacade[] enemyTeam = new EnemyFacade[NumericConstants.PlayersInTeam];

            for (int i = 0; i < enemyTeam.Length; i++)
            {
                EnemyFacade enemy = _gameObjectFactory.CreateEnemy().GetComponent<EnemyFacade>();
                enemy.Initialize(ball);
                enemyTeam[i] = enemy;
            }

            return enemyTeam;
        }

        private Referee SpawnReferee(Ball ball)
        {
            Referee referee = _gameObjectFactory.CreateReferee();
            referee.Initialize(ball);
            return referee;
        }

        private CinemachineBrain SpawnCamera() =>
            _gameObjectFactory.CreateCamera();


        private Ball SpawnBall() =>
            _gameObjectFactory.CreateBall();


        private GameplayHUD SpawnHUD() =>
            _gameObjectFactory.CreateHUD(); //TODO different for different platforms


        private CinemachineVirtualCamera SpawnVirtualCamera() =>
            _gameObjectFactory.CreateCinemachineVirtualCamera();
    }
}