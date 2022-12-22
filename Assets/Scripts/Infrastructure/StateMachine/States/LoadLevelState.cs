using Gameplay.Camera;
using Gameplay.Player.MonoBehaviour.Brain;
using Infrastructure.Factory;
using Modules.StateMachine;
using UI;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class LoadLevelState : IParameterState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameObjectFactory _gameObjectFactory;
        
        private LoadingCurtain _loadingCurtain;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IGameObjectFactory gameObjectFactory)
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
            Transform playerSpawnPoint = GameObject.FindWithTag("PlayerSpawnPoint").transform;    //TODO: BURN this
            Transform enemyBasket = GameObject.FindWithTag("EnemyBasket").transform;  
         
            GameObject player = _gameObjectFactory.CreatePlayer(playerSpawnPoint.position);
            GameObject camera = _gameObjectFactory.CreateCamera();
            
            CameraTargetTracker cameraTargetTracker = camera.GetComponent<CameraTargetTracker>();
            cameraTargetTracker.SetTarget(player.transform);
            CameraFocuser cameraFocuser = camera.GetComponent<CameraFocuser>();
            cameraFocuser.SetTarget(enemyBasket);
            player.GetComponent<InputControlledBrain>().SetTargetTracker(cameraTargetTracker);
            
            _gameObjectFactory.CreateHUD();               //TODO different for different platforms
            
            _gameStateMachine.Enter<GamePlayState>();
        }
    }
}