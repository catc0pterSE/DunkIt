using System;
using Gameplay.Effects;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw
{
    using Camera = UnityEngine.Camera;

    public class BallThrower : SwitchableComponent
    {
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private TrajectoryDrawer _trajectoryDrawer;
        [Range(0.1f, 10f)] [SerializeField] private float _flightTime;
        [Range(0, 1000)] [SerializeField] private float _maxBallSpeed = 14;
        [SerializeField] private BallLandingEffect _ballLandingEffect;

        private Vector3 _destinationPoint;
        private Camera _camera;
        private IInputService _inputService;
        private Ball.MonoBehavior.Ball _ball;

        public event Action BallThrown;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();

        private void OnEnable()
        {
            _trajectoryDrawer.Enable();
        }

        private void OnDisable()
        {
            _trajectoryDrawer.StopDrawing();
            _trajectoryDrawer.Disable();
            DisableLandingEffect();
        }

        private void Awake()
        {
            _ballLandingEffect = Instantiate(_ballLandingEffect);
            _ballLandingEffect.Disable();
        }

        private void Update()
        {
            if (TrySetDestination())
            {
                EnableLandingEffect();
                Vector3 launchVelocity = CalculateLaunchVelocity();
                _trajectoryDrawer.Draw(_ballPosition.position, launchVelocity);
                Throw(launchVelocity);
            }
            else
            {
                _trajectoryDrawer.StopDrawing();
            }
        }

        public void Initialize(Ball.MonoBehavior.Ball ball, Camera gameplayCamera)
        {
            _ball = ball;
            _camera = gameplayCamera;
        }

        private bool TrySetDestination()
        {
            Ray ray = _camera.ScreenPointToRay(InputService.PointerPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _destinationPoint = hit.point;
                _ballLandingEffect.Settle(hit);
                return true;
            }
            
            _ballLandingEffect.Disable();
            return false;
        }

        private void EnableLandingEffect() =>
            _ballLandingEffect.Enable();
        
        private void DisableLandingEffect() =>
            _ballLandingEffect.Disable();
        
        private Vector3 CalculateLaunchVelocity()
        {
            Vector3 toTarget = _destinationPoint - _ballPosition.transform.position;
            float gSquared = Physics.gravity.sqrMagnitude;
            float potentialEnergy = _maxBallSpeed * _maxBallSpeed + Vector3.Dot(toTarget, Physics.gravity);
            float discriminant = potentialEnergy * potentialEnergy - gSquared * toTarget.sqrMagnitude;

            if (discriminant < 0)
            {
                return Vector3.zero;
            }

            float discriminantRoot = Mathf.Sqrt(discriminant);
            float maxFlightTime = Mathf.Sqrt((potentialEnergy + discriminantRoot) * NumericConstants.Two / gSquared);
            float minFlightTime = Mathf.Sqrt((potentialEnergy - discriminantRoot) * NumericConstants.Two / gSquared);

            _flightTime = Mathf.Clamp(_flightTime, minFlightTime, maxFlightTime);

            Vector3 velocity = toTarget / _flightTime - Physics.gravity * (_flightTime * NumericConstants.Half);

            return velocity;
        }

        private void Throw(Vector3 velocity)
        {
            if (InputService.Clicked == false)
                return;
            
            if (_ball == null)
                return;

            _ball.Throw(velocity);
            BallThrown?.Invoke();
        }
    }
}