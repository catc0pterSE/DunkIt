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

    public class BallThrower : SwitchableComponent
    {
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private TrajectoryDrawer _trajectoryDrawer;
        
        private IInputService _inputService;
        private Ball _ball;
        private Vector3 _launchVelocity;

        public event Action BallThrown;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();

        private void OnEnable()
        {
            _ball.Disable();
            InputService.PointerUp += Throw;
            _trajectoryDrawer.Enable();
            _launchVelocity = Vector3.zero;
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
                _launchVelocity.x -= InputService.PointerMovement.x;
                _launchVelocity.y += InputService.PointerMovement.y;

                _launchVelocity = Vector3.ProjectOnPlane(_launchVelocity, _ballPosition.right);

                _trajectoryDrawer.Draw(_ballPosition.position, _launchVelocity);
            }
            else
            {
                _launchVelocity = Vector3.zero;
                _trajectoryDrawer.Disable();
            }
        }

        public void Initialize(Ball ball)
        {
            _ball = ball;
        }

        private void Throw()
        {
            Ball cloneBall = Instantiate(_ball, _ballPosition);
            cloneBall.Enable();
            cloneBall.Throw(_launchVelocity);
            //BallThrown?.Invoke();
        }
    }
}