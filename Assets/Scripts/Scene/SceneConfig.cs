using System;
using UnityEngine;

namespace Scene
{
    using UnityEngine.SceneManagement;

    public class SceneConfig : MonoBehaviour
    {
        [SerializeField] private Transform _playerBucket;
        [SerializeField] private Transform _enemyBasket;

        private Scene _simulationScene;

        private bool _isSimulationSceneInitialized = false;

        public Transform PlayerBasket => _playerBucket;
        public Transform EnemyBasket => _enemyBasket;

        public Scene SimulationScene => _isSimulationSceneInitialized ? _simulationScene : CreatePhysicsScene();

        private Scene CreatePhysicsScene()
        {
            Scene simulationScene =
                SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));

            Collider[] obstacles = GetComponentsInChildren<Collider>();
            
            foreach (Collider obstacle in obstacles)
            {
                Transform obstacleTransform = obstacle.transform;
                
                GameObject clonedObstacle = Instantiate(obstacle.gameObject, obstacleTransform.position, obstacleTransform.rotation);
                clonedObstacle.transform.localScale = obstacleTransform.lossyScale;

                if (clonedObstacle.TryGetComponent(out Renderer objectRenderer))
                    objectRenderer.enabled = false;
                
                SceneManager.MoveGameObjectToScene(clonedObstacle, simulationScene);
            }

            _simulationScene = simulationScene;
            _isSimulationSceneInitialized = true;

            return simulationScene;
        }
    }
}