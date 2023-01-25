using Gameplay.Ball.MonoBehavior;
using Infrastructure.Factory;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace z_Test
{
    public class SImulationTrajectoryDrawer : SwitchableMonoBehaviour
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private int _maxPhysicsFrameIterations = 100;
        [SerializeField] private float _simulationStep = 0.02f;

        private Transform _obstaclesParent;
        private Ball _ballSimulation;
        private UnityEngine.SceneManagement.Scene _simulationScene;
        private PhysicsScene _physicsScene;


        private Ball BallSimulation => _ballSimulation ??= CreateBallSimulation();

        public void SetSimulationScene(UnityEngine.SceneManagement.Scene simulationScene)
        {
            _simulationScene = simulationScene;
            _physicsScene = simulationScene.GetPhysicsScene();
        }

        public void SimulateTrajectory(Vector3 startPosition, Vector3 velocity)
        {
            BallSimulation.ZeroVelocity();
            BallSimulation.transform.position = startPosition;

            BallSimulation.Throw(velocity);

            _line.positionCount = _maxPhysicsFrameIterations;

            for (var i = 0; i < _maxPhysicsFrameIterations; i++)
            {
                _physicsScene.Simulate(_simulationStep);
                _line.SetPosition(i, BallSimulation.transform.position);
            }
        }

        private Ball CreateBallSimulation()
        {
            Ball ball = Services.Container.Single<IGameObjectFactory>().CreateBall();
            SceneManager.MoveGameObjectToScene(ball.gameObject, _simulationScene);
            ball.StopRendering();
            return ball;
        }
    }

    public class SceneConfig: UnityEngine.MonoBehaviour
    {

        private bool _isSimulationSceneInitialized = false;
        private UnityEngine.SceneManagement.Scene _simulationScene;
        public UnityEngine.SceneManagement.Scene SimulationScene => _isSimulationSceneInitialized ? _simulationScene : CreatePhysicsScene();

        private UnityEngine.SceneManagement.Scene CreatePhysicsScene()
        {
            UnityEngine.SceneManagement.Scene simulationScene =
                SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));

            Collider[] obstacles = GetComponentsInChildren<Collider>();

            foreach (Collider obstacle in obstacles)
            {
                Transform obstacleTransform = obstacle.transform;

                GameObject clonedObstacle = Instantiate(obstacle.gameObject, obstacleTransform.position,
                    obstacleTransform.rotation);
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