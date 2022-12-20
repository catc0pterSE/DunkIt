using Camera;
using Infrastructure.StateMachine.States;
using Player;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState : IPayLoadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(string name)
        {
            _sceneLoader.LoadScene(name, OnLoaded);
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            Transform playerSpawnPoint = GameObject.FindWithTag("PlayerSpawnPoint").transform; //TODO: BURN this all
            Transform enemyBasket = GameObject.FindWithTag("EnemyBasket").transform;
            GameObject player = Instantiate("Player/Player", playerSpawnPoint.position);
            GameObject camera = Instantiate("Camera/Camera");
            CameraTargetTracker cameraTargetTracker = camera.GetComponent<CameraTargetTracker>();
            cameraTargetTracker.SetTarget(player.transform);
            CameraFocuser cameraFocuser = camera.GetComponent<CameraFocuser>();
            cameraFocuser.SetTarget(enemyBasket);
            player.GetComponent<InputPlayerMover>().SetTargetTracker(cameraTargetTracker);
            Instantiate("UI/HUD/HUD"); //TODO different for different platforms
        }

        private static GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        private static GameObject Instantiate(string path, Vector3 point)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, point, Quaternion.identity);
        }
    }
}