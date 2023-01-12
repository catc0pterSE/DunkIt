using Gameplay.Ball.MonoBehavior;
using Gameplay.Camera;
using Gameplay.Camera.MonoBehaviour;
using Gameplay.NPC.EnemyPlayer;
using Gameplay.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.NPC.Referee.MonoBehaviour;
using Gameplay.Player.MonoBehaviour;
using Gameplay.StateMachine;
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

        private LoadingCurtain _loadingCurtain;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            IGameObjectFactory gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
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
            CameraFacade camera = SpawnCamera();
            PlayerFacade[] playerTeam = SpawnPlayerTeam(camera);
            EnemyFacade[] enemyTeam = SpawnEnemyTeam();
            Referee referee = SpawnReferee();
            Ball ball = SpawnBall();

            GameplayLoopStateMachine gameplayLoopStateMachine =
                new GameplayLoopStateMachine(playerTeam, enemyTeam, referee, ball, camera, sceneConfig, _gameStateMachine);

            _gameStateMachine.Enter<GamePlayLoopState, GameplayLoopStateMachine>(gameplayLoopStateMachine);
        }

        private PlayerFacade[] SpawnPlayerTeam(CameraFacade cameraFacade)
        {
            PlayerFacade[] playerTeam = new PlayerFacade[NumericConstants.PlayersInTeam];

            for (int i = 0; i < playerTeam.Length; i++)
            {
                playerTeam[i] = _gameObjectFactory.CreatePlayer().GetComponent<PlayerFacade>();
                playerTeam[i].SetCamera(cameraFacade.transform);
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

        private CameraFacade SpawnCamera()
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