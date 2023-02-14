using System;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using Scene.Ring;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw
{
    using Ball.MonoBehavior;

    public class testBallThrower : SwitchableComponent
    {
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private TrajectoryDrawer _trajectoryDrawer;
        [SerializeField] private float _forceIncreaseSpeed = 1;
        [SerializeField] private float _rotateIncreaseSpeed = 1;

        private float _launchForce;
        private IInputService _inputService;
        private Ball _ball;
        private Vector3 _launchVector;
        private Ring _enemyRing;

        public event Action BallThrown;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();

        private void OnEnable()
        {
            _trajectoryDrawer.Enable();
            _launchVector = (_enemyRing.BallDunkPoint.position - _ballPosition.transform.position).normalized;
        }

        private void OnDisable()
        {
            _trajectoryDrawer.Disable();
        }

        private void Update()
        {
            if (InputService.TouchHeldDown)
            {
                _launchForce += InputService.InputDirection.x*_forceIncreaseSpeed;
                _ballPosition.Rotate(Vector3.left, InputService.InputDirection.y*_rotateIncreaseSpeed);
                _trajectoryDrawer.Draw(_ballPosition.position, _ballPosition.forward*_launchForce);
            }
        }

        public void Initialize(Ball ball, Ring enemyRing)
        {
            _enemyRing = enemyRing;
            _ball = ball;
        }
      
        private void CalculateLaunchVector()
        {
           
        }

        private void Throw()
        {
            if (_launchVector == Vector3.zero)
                return;

            _ball.Throw(_launchVector*_launchForce);
            BallThrown?.Invoke();
        }
    }
}