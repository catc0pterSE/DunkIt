using System;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Gameplay.Effects;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;

namespace z_Test
{
    using Camera = UnityEngine.Camera;

    public class classicBallThrower : SwitchableComponent
    {
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private TrajectoryDrawer _trajectoryDrawer;
        [Range(0.1f, 10f)] [SerializeField] private float _flightTime;
        [Range(0, 1000)] [SerializeField] private float _maxBallSpeed = 14;
        [SerializeField] private BallLandingEffect _ballLandingEffect;
        [SerializeField] private float _curveChangingSpeed = 8;
        [SerializeField] private LayerMask _raycastTargetLayerMask;

        private Vector3 _destinationPoint;
        private Camera _camera;
        private IInputService _inputService;
        private Ball _ball;
        private Vector3 _launchVelocity;

        public event Action BallThrown;

        //private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();

        private void OnEnable()
        {
            Time.timeScale = 0.3f; // TODO: determine
            _trajectoryDrawer.Enable();
           _inputService.ThrowButtonDown += Throw;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
            _trajectoryDrawer.StopDrawing();
            _trajectoryDrawer.Disable();
            DisableLandingEffect();
            _inputService.ThrowButtonDown -= Throw;
        }

        private void Awake()
        {
            _ballLandingEffect = Instantiate(_ballLandingEffect); //TODO: to gameobj factory
            _ballLandingEffect.Disable();
        }

        private void Update()
        {
            if (_inputService.PointerHeldDown)
            {
                SetDestination();
                EnableLandingEffect();
            }

            CalculateLaunchVelocity();

            if (_destinationPoint != Vector3.zero)
                _trajectoryDrawer.Draw(_ballPosition.position, _launchVelocity);

            AdjustFlyingTime();
        }

        public void Initialize(Ball ball, Camera gameplayCamera)
        {
            _ball = ball;
            _camera = gameplayCamera;
        }

        private void AdjustFlyingTime()
        {
            _flightTime += _inputService.ThrowCurve * _curveChangingSpeed;
        }

        private void SetDestination()
        {
            Ray ray = _camera.ScreenPointToRay(_inputService.PointerPosition);

            bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

            if (Physics.Raycast(ray, out RaycastHit hit, Single.PositiveInfinity, _raycastTargetLayerMask) &&
                isOverUI == false)
            {
                _destinationPoint = hit.point;
                _ballLandingEffect.Settle(hit);
            }
        }

        private void EnableLandingEffect() =>
            _ballLandingEffect.Enable();

        private void DisableLandingEffect() =>
            _ballLandingEffect.Disable();

        private void CalculateLaunchVelocity()
        {
            if (_destinationPoint == Vector3.zero)
            {
                _launchVelocity = Vector3.zero;
                return;
            }

            Vector3 toTarget = _destinationPoint - _ballPosition.transform.position;
            float gSquared = Physics.gravity.sqrMagnitude;
            float potentialEnergy = _maxBallSpeed * _maxBallSpeed + Vector3.Dot(toTarget, Physics.gravity);
            float discriminant = potentialEnergy * potentialEnergy - gSquared * toTarget.sqrMagnitude;

            if (discriminant < 0)
            {
                _launchVelocity = Vector3.zero;
                return;
            }

            float discriminantRoot = Mathf.Sqrt(discriminant);
            float maxFlightTime = Mathf.Sqrt((potentialEnergy + discriminantRoot) * NumericConstants.Double / gSquared);
            float minFlightTime = Mathf.Sqrt((potentialEnergy - discriminantRoot) * NumericConstants.Double / gSquared);

            _flightTime = Mathf.Clamp(_flightTime, minFlightTime, maxFlightTime);

            Vector3 velocity = toTarget / _flightTime - Physics.gravity * (_flightTime * NumericConstants.Half);

            _launchVelocity = velocity;
        }

        private void Throw()
        {
            if (_launchVelocity == Vector3.zero)
                return;

            _ball.Fly(_launchVelocity);
            BallThrown?.Invoke();
        }
    }
}