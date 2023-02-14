using System;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using Scene.Ring;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw
{
    using Ball.MonoBehavior;

    public class testBallThrower : SwitchableComponent
    {
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private TrajectoryDrawer _trajectoryDrawer;
        [SerializeField] private float _forceChangeSpeed = 1;
        [SerializeField] private float _rotateSpeed = 1;

        private float _launchForce;
        private IInputService _inputService;
        private Ball _ball;
        private Vector3 _launchVector;
        private Ring _enemyRing;
        private float _currentAngle;

        public event Action BallThrown;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();

        private void OnEnable()
        {
            InputService.PointerUp += Throw;
            _trajectoryDrawer.Enable();
            _currentAngle = 0;
        }

        private void OnDisable()
        {
            InputService.PointerUp -= Throw;
            _trajectoryDrawer.Disable();
        }

        private void Update()
        {
            if (InputService.PointerHeldDown)
            {
                _launchForce = Mathf.Clamp(
                    _launchForce - InputService.MovementInput.x * _forceChangeSpeed * Time.deltaTime, 0f,
                    1000f);

                _currentAngle += -InputService.MovementInput.y * _rotateSpeed * Time.deltaTime;
                _currentAngle *= Mathf.Rad2Deg;
                _currentAngle = Mathf.Clamp(_currentAngle, -80, 80);
                _currentAngle *= Mathf.Deg2Rad;
                _ballPosition.localRotation = quaternion.Euler(_currentAngle, 0, 0);


                _trajectoryDrawer.Draw(_ballPosition.position, _ballPosition.forward * _launchForce / _ball.Mass);
            }
            else
            {
                _launchForce = 10;
                _currentAngle = 0;
                _trajectoryDrawer.Disable();
            }
        }

        public void Initialize(Ball ball, Ring enemyRing)
        {
            _enemyRing = enemyRing;
            _ball = ball;
        }

        private void Throw()
        {
            Ball cloneBall = Instantiate(_ball, _ballPosition);
            cloneBall.Throw(_ballPosition.forward * _launchForce);
            //BallThrown?.Invoke();
        }
    }
}