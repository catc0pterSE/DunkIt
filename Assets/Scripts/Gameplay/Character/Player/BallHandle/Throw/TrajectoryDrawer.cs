using Infrastructure.Factory;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Character.Player.BallHandle.Throw
{
    using UnityEngine.SceneManagement;
    using Ball.MonoBehavior;

    public class TrajectoryDrawer : SwitchableMonoBehaviour
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private int _maxPhysicsFrameIterations = 100;

        private Transform _obstaclesParent;
        private Ball _ballSimulation;
        private Scene _simulationScene;
        private PhysicsScene _physicsScene;

        private Ball BallSimulation => _ballSimulation ??= CreateBallSimulation();

        public void SetSimulationScene(Scene simulationScene)
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
                _physicsScene.Simulate(Time.fixedDeltaTime);
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
}