using System;
using System.Collections;
using Infrastructure.Input.InputService;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw
{
    using Ball.MonoBehavior;

    public class InputBallThrower : SwitchableComponent
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
        private Coroutine _aimRoutine;
        private float CameraPositionMultiplier => _camera.transform.position.z < transform.position.z ? 1 : -1;

        private void OnEnable()
        {
            _inputService.PointerDown += StartAim;
            _trajectoryDrawer.Enable();
            _launchVelocity = Vector3.zero;
        }

        private void OnDisable()
        {
            _inputService.PointerDown -= StartAim;
            _trajectoryDrawer.Disable();
        }

        public void Initialize(Ball ball, UnityEngine.Camera gameplayCamera, IInputService inputService)
        {
            _inputService = inputService;
            _camera = gameplayCamera;
            _ball = ball;
        }

        private void Throw()
        {
            _ball.Fly(_launchVelocity);
            BallThrown?.Invoke();
        }

        private void StartAim()
        {
            StopPointerUpTracking();
            _aimRoutine = StartCoroutine(Aim());
        }

        private void StopPointerUpTracking()
        {
            if (_aimRoutine !=null)
                StopCoroutine(_aimRoutine);
        }

        private IEnumerator Aim()
        {
            _inputService.PointerUp += Throw;
            
            while (_inputService.PointerHeldDown)
            {
                Vector3 normalizedPointerMovement =  _inputService.PointerMovement.normalized;
                
                _launchVelocity.x += normalizedPointerMovement.x*Time.deltaTime*_launchVelocityXSense*CameraPositionMultiplier;
                _launchVelocity.y += normalizedPointerMovement.y*Time.deltaTime*_launchVelocityYSense;

                _launchVelocity = Vector3.ProjectOnPlane(_launchVelocity, _ballPosition.right);

                _trajectoryDrawer.Draw(_ballPosition.position, _launchVelocity);
                
                yield return null;
            }
            
            _inputService.PointerUp -= Throw;
        }
    }
}