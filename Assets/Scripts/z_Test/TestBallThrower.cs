using Gameplay.Ball.MonoBehavior;
using Gameplay.Character;
using Gameplay.Character.Player.BallHandle.Throw;
using Infrastructure.Factory;
using Infrastructure.Input;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;

namespace z_Test
{
    public class TestBallThrower : SwitchableMonoBehaviour
    {
        [SerializeField] private TrajectoryDrawer _trajectoryDrawer;
        [SerializeField] private float _force = 20;
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private float _rotateSpeed = 50;

        private Ball _ball;
        private IInputService _inputService;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();
        private Vector2 InputDirection => InputService.InputDirection;

        public void SetBall(Ball ball) =>
            _ball = ball;

        private void Update()
        {
            Aim();
            /*_trajectoryDrawer.Draw(_ball, _ballPosition.position, _ballPosition.forward * _force);*/
            Throw();
        }

        private void Aim()
        {
            Vector3 rotateDirection = new Vector3(InputDirection.y, -InputDirection.x, 0);
            _ballPosition.transform.Rotate(-rotateDirection * (_rotateSpeed * Time.deltaTime));
        }

        private void Throw()
        {
            if (InputService.Clicked == false)
                return;

            Ball ball = Services.Container.Single<IGameObjectFactory>().CreateBall(); //TODO: this is for tests
            ball.SetOwner(this.GetComponent<Character>());
            ball.Throw(_ballPosition.forward * _force);
            
            
            // _ball.Throw(_ballPosition.forward * _force);
        }
    }
}