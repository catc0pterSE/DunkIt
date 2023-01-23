using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Character.Player.BallHandle.Throw
{
    using Ball.MonoBehavior;
    public class TrajectoryDrawer : SwitchableMonoBehaviour
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private int _maxPhysicsFrameIterations = 100;
        
        private Transform _obstaclesParent;
        private Ball _ballSimulation;

        private UnityEngine.SceneManagement.Scene _simulationScene;
        private PhysicsScene _physicsScene;

        public void SetSimulationScene(UnityEngine.SceneManagement.Scene simulationScene)
        {
            _simulationScene = simulationScene;
            _physicsScene = simulationScene.GetPhysicsScene();
        }

        public void SimulateTrajectory(Ball ball, Vector3 startPosition, Vector3 velocity)
        {
            Ball ghostBall = Instantiate(ball, startPosition, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(ghostBall.gameObject, _simulationScene);

            ghostBall.Throw(velocity);

            _line.positionCount = _maxPhysicsFrameIterations;

            for (var i = 0; i < _maxPhysicsFrameIterations; i++) 
            {
                _physicsScene.Simulate(Time.fixedDeltaTime);
                _line.SetPosition(i, ghostBall.transform.position);
            }

            Destroy(ghostBall.gameObject);
        }
    }
}