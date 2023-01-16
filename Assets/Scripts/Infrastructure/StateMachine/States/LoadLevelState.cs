using Cinemachine;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
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
            SpawnHUD();
            SceneConfig sceneConfig = GameObject.FindObjectOfType<SceneConfig>();
            CinemachineBrain camera = SpawnCamera();
            PlayerFacade[] playerTeam = SpawnPlayerTeam();
            EnemyFacade[] enemyTeam = SpawnEnemyTeam();
            Referee referee = SpawnReferee();
            Ball ball = SpawnBall();

            GameplayLoopStateMachine gameplayLoopStateMachine =
                new GameplayLoopStateMachine(playerTeam, enemyTeam, referee, ball, camera, sceneConfig, _coroutineRunner,  _gameStateMachine);

            _gameStateMachine.Enter<GamePlayLoopState, GameplayLoopStateMachine>(gameplayLoopStateMachine);
        }

        private PlayerFacade[] SpawnPlayerTeam()
        {
            PlayerFacade[] playerTeam = new PlayerFacade[NumericConstants.PlayersInTeam];

            for (int i = 0; i < playerTeam.Length; i++)
            {
                playerTeam[i] = _gameObjectFactory.CreatePlayer().GetComponent<PlayerFacade>();
            }

            return playerTeam;
        }

        private EnemyFacade[] SpawnEnemyTeam()
        {
            EnemyFacade[] enemyTeam = new EnemyFacade[NumericConstants.PlayersInTeam];

            for (int i = 0; i < enemyTeam.Length; i++)
            {
                enemyTeam[i] = _gameObjectFactory.CreateEnemy().GetComponent<EnemyFacade>();
            }

            return enemyTeam;
        }

        private Referee SpawnReferee()
        {
            return _gameObjectFactory.CreateReferee();
        }

        private CinemachineBrain SpawnCamera()
        {
            return _gameObjectFactory.CreateCamera();
        }

        private Ball SpawnBall()
        {
            return _gameObjectFactory.CreateBall();
        }

        private void SpawnHUD()
        {
            _gameObjectFactory.CreateHUD(); //TODO different for different platforms
        }
    }
}