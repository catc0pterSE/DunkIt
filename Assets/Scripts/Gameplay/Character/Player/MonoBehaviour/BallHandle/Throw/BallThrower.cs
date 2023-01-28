using System;
using Gameplay.Effects;
using Infrastructure.Factory;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;


namespace Gameplay.Character.Player.BallHandle.Throw
{
    using Ball.MonoBehavior;
    using Camera = UnityEngine.Camera;

    public class BallThrower : SwitchableMonoBehaviour
    {
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private TrajectoryDrawer _trajectoryDrawer;
        [Range(0.1f, 10f)] [SerializeField] private float _flightTime;
        [Range(0, 1000)] [SerializeField] private float _maxBallSpeed = 14;
        [SerializeField] private BallLandingEffect _ballLandingEffect;
        
        private Vector3 _destinationPoint;
        private Camera _camera;
        private IInputService _inputService;
        private Ball _ball;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();

        private void Awake()
        {
            _ballLandingEffect = Instantiate(_ballLandingEffect);
            _ballLandingEffect.Disable();
        }

        private void Update()
        {
            SetDestination();
            Vector3 launchVelocity = CalculateLaunchVelocity();
            _trajectoryDrawer.Draw(_ballPosition.position, launchVelocity);
            Throw(launchVelocity);
        }

        public void Initialize(Ball ball, Camera gameplayCamera)
        {
            _ball = ball;
            _camera = gameplayCamera;
        }

        private void SetDestination()
        {
            Ray ray = _camera.ScreenPointToRay(InputService.PointerPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _destinationPoint = hit.point;
                _ballLandingEffect.Enable();
                _ballLandingEffect.Settle(hit);
                return;
            }
            
            _ballLandingEffect.Disable();
        }

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
            
            //float lowestEnergyFlightTime = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gSquared));
            
            _flightTime = Mathf.Clamp(_flightTime, minFlightTime, maxFlightTime);
            
            Vector3 velocity = toTarget / _flightTime - Physics.gravity * (_flightTime * NumericConstants.Half);

            return velocity;
        }

        private void Throw(Vector3 velocity)
        {
            if (InputService.Clicked == false)
                return;

            Ball ball = Services.Container.Single<IGameObjectFactory>().CreateBall(); //TODO: this is for tests
            ball.SetOwner(this.GetComponent<Character>());
            ball.Throw(velocity);

            // _ball.Throw(_ballPosition.forward * _force);
        }
    }
}