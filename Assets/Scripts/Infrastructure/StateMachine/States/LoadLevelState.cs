using Camera;
using Infrastructure.Factory;
using Infrastructure.ServiceManagement;
using Player;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using Utility.Static;

namespace Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayLoadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        
        private IGameObjectFactory _gameObjectFactory;
        private LoadingCurtain _loadingCurtain;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        private IGameObjectFactory GameObjectFactory =>
            _gameObjectFactory ??= Services.Container.Single<IGameObjectFactory>();

        private LoadingCurtain LoadingCurtain => _loadingCurtain ??=
            GameObjectFactory.CreateLoadingCurtain().GetComponent<LoadingCurtain>();

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
         
            GameObject player = GameObjectFactory.CreatePlayer(playerSpawnPoint.position);
            GameObject camera = GameObjectFactory.CreateCamera();
            
            CameraTargetTracker cameraTargetTracker = camera.GetComponent<CameraTargetTracker>();
            cameraTargetTracker.SetTarget(player.transform);
            CameraFocuser cameraFocuser = camera.GetComponent<CameraFocuser>();
            cameraFocuser.SetTarget(enemyBasket);
            player.GetComponent<InputPlayerMover>().SetTargetTracker(cameraTargetTracker);
            
            GameObjectFactory.CreateHUD();               //TODO different for different platforms
            
            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}