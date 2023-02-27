using System;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw
{
    using Ball.MonoBehavior;

    public class BallThrower : SwitchableComponent
    {
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private TrajectoryDrawer _trajectoryDrawer;
        [SerializeField] private float _launchVelocityXSense = 80;
        [SerializeField] private float _launchVelocityYSense = 80;
        
        private IInputService _inputService;
        private Ball _ball;
        private Vector3 _launchVelocity;
        private UnityEngine.Camera _camera;
        public event Action BallThrown;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();
        private float CameraPositionMultiplier => _camera.transform.position.z < transform.position.z ? 1 : -1;

        private void OnEnable()
        {
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
                Vector3 normalizedPointerMovement = InputService.PointerMovement.normalized;
                
                _launchVelocity.x += normalizedPointerMovement.x*Time.deltaTime*_launchVelocityXSense*CameraPositionMultiplier;
                _launchVelocity.y += normalizedPointerMovement.y*Time.deltaTime*_launchVelocityYSense;

                _launchVelocity = Vector3.ProjectOnPlane(_launchVelocity, _ballPosition.right);

                _trajectoryDrawer.Draw(_ballPosition.position, _launchVelocity);
            }
            else
            {
                _launchVelocity = Vector3.zero;
                _trajectoryDrawer.StopDrawing();
            }
        }

        public void Initialize(Ball ball, UnityEngine.Camera gameplayCamera, IInputService inputService)
        {
            _inputService = inputService;
            _camera = gameplayCamera;
            _ball = ball;
        }

        private void Throw()
        {
            _ball.Throw(_launchVelocity);
            BallThrown?.Invoke();
        }
    }
}