using Infrastructure.Input;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.BallHandle.Throw
{
    using Ball.MonoBehavior;
    public class BallThrower : SwitchableMonoBehaviour
    {
        [SerializeField] private TrajectoryDrawer _trajectory;
         
        [SerializeField] private float _force = 20;
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private float _rotateSpeed = 50;

        private Ball _ball;
        private IInputService _inputService;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();
        private Vector3 InputDirection => InputService.InputDirection;

        public void SetBall(Ball ball) =>
            _ball = ball;
        
        private void Update()
        {
            Aim();
            _trajectory.SimulateTrajectory(_ball, _ballPosition.position, _ballPosition.forward * _force);
            Trow();
        }

        private void Aim()
        {
            Vector3 rotateDirection = new Vector3(InputDirection.x, InputDirection.y, 0);
            _ballPosition.transform.Rotate(-rotateDirection * (_rotateSpeed * Time.deltaTime));
        }

        private void Trow()
        {
            if (InputService.Clicked)
                _ball.Throw(_ballPosition.forward * _force);
        }
    }
}