using System;
using System.Collections;
using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using Infrastructure.Input.InputService;
using Modules.MonoBehaviour;
using Scene.Ring;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw
{
    using Ball.MonoBehavior;

    public class LocalControlledThrowing : SwitchableComponent
    {
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private TrajectoryDrawer _trajectoryDrawer;
        [SerializeField] private float _launchVelocityXSense = 80;
        [SerializeField] private float _launchVelocityYSense = 80;
        [SerializeField] private BallThrower _ballThrower;
        [SerializeField] private float _defaultCameraXOffset;
        [SerializeField] private PlayerMover _mover;

        private IInputService _inputService;
        private Vector3 _inputLaunchVelocity;
        private UnityEngine.Camera _camera;
        private Coroutine _aimRoutine;
        private Ring _oppositeRing;

        public void Initialize(UnityEngine.Camera gameplayCamera, IInputService inputService, Ring oppositeRing)
        {
            _oppositeRing = oppositeRing;
            _inputService = inputService;
            _camera = gameplayCamera;
        }

        private float CameraPositionMultiplier => _camera.transform.position.z < transform.position.z ? 1 : -1;

        private void OnEnable()
        {
            SetUpRingCamera();
            _mover.RotateTo(_oppositeRing.transform.position, StartThrow);
        }

        private void OnDisable()
        {
            _inputService.PointerDown -= StartAim;
            _trajectoryDrawer.Disable();
        }
        
        private void StartThrow()
        {
            _inputService.PointerDown += StartAim;
            _trajectoryDrawer.Enable();
            _inputLaunchVelocity = Vector3.zero;  
        }
        
        private void SetUpRingCamera()
        {
            Transform playerTransform = transform;
            Transform ringTransform = _oppositeRing.transform;
            CinemachineVirtualCamera ringCamera = _oppositeRing.VirtualCamera;
            _oppositeRing.RingTargetGroup.m_Targets[1].target = playerTransform;
            ringCamera.Follow = playerTransform;
            ringCamera.LookAt = _oppositeRing.RingTargetGroup.Transform;
            CinemachineFramingTransposer framingTransposer =
                ringCamera.GetCinemachineComponent<CinemachineFramingTransposer>()
                ?? throw new NullReferenceException("There is no framingTransposer on RingVirtualCamera");

            framingTransposer.m_TrackedObjectOffset.x =
                ringTransform.position.z > playerTransform.position.z != _oppositeRing.IsFlipped
                    ? -_defaultCameraXOffset
                    : _defaultCameraXOffset;

            ringCamera.Prioritize();
        }

        private void ThrowWithInputVelocity() =>
            _ballThrower.Throw(_inputLaunchVelocity);

        private void StartAim()
        {
            StopPointerUpTracking();
            _aimRoutine = StartCoroutine(Aim());
        }

        private void StopPointerUpTracking()
        {
            if (_aimRoutine != null)
                StopCoroutine(_aimRoutine);
        }

        private IEnumerator Aim()
        {
            _inputService.PointerUp += ThrowWithInputVelocity;

            while (_inputService.PointerHeldDown)
            {
                Vector3 normalizedPointerMovement = _inputService.PointerMovement.normalized;

                _inputLaunchVelocity.x += normalizedPointerMovement.x * Time.deltaTime * _launchVelocityXSense *
                                          CameraPositionMultiplier;
                _inputLaunchVelocity.y += normalizedPointerMovement.y * Time.deltaTime * _launchVelocityYSense;

                _inputLaunchVelocity = Vector3.ProjectOnPlane(_inputLaunchVelocity, _ballPosition.right);

                _trajectoryDrawer.Draw(_ballPosition.position, _inputLaunchVelocity);

                yield return null;
            }

            _inputService.PointerUp -= ThrowWithInputVelocity;
        }
    }
}