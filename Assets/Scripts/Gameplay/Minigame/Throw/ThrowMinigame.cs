using System;
using System.Collections;
using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour;
using Modules.MonoBehaviour;
using Scene;
using Scene.Ring;
using UI;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Minigame.Throw
{
    using Ball.MonoBehavior;

    public class ThrowMinigame : SwitchableMonoBehaviour, IMinigame
    {
        [SerializeField] private ThrowUI _interface;
        [SerializeField] private float _ballTrackingSeconds = 5;
        [SerializeField] private float _defaultCameraXOffset = 10;
        [SerializeField] private int _cameraTargetGroupPlayerIndex = 1;

        private WaitForSeconds _waitForGoalTrackingTime;
        private PlayerFacade _throwingPlayer;
        private Ball _ball;
        private Coroutine _trackingGoal;
        private Ring _ring;

        public event Action Won;
        public event Action Lost;

        private void OnDisable()
        {
            StopGoalTracking();
            UnsubscribeFromThrowingPlayer();
        }

        public void Initialize
        (
            PlayerFacade throwingPlayer,
            Ball ball
        )
        {
            _ring = throwingPlayer.OppositeRing;
            _throwingPlayer = throwingPlayer;
            _ball = ball;
            _waitForGoalTrackingTime = new WaitForSeconds(_ballTrackingSeconds);
        }

        public void Launch()
        {
            SetUpRingCamera();
            SubscribeOnThrowingPlayer();
            _interface.Enable();
        }

        private void SubscribeOnThrowingPlayer() =>
            _throwingPlayer.BallThrown += OnBallThrown;

        private void UnsubscribeFromThrowingPlayer() =>
            _throwingPlayer.BallThrown -= OnBallThrown;

        private void OnBallThrown()
        {
            _interface.Disable();
            _ring.VirtualCamera.LookAt = _ball.transform;
            StartGoalTracking();
        }

        private void StartGoalTracking()
        {
            StopGoalTracking();
            _trackingGoal = StartCoroutine(TrackGoal());
        }

        private void StopGoalTracking()
        {
            if (_trackingGoal != null)
                StopCoroutine(_trackingGoal);

            UnsubscribeFromEnemyRing();
        }

        private IEnumerator TrackGoal()
        {
            SubscribeOnEnemyRing();
            yield return _waitForGoalTrackingTime;
            UnsubscribeFromEnemyRing();
            OnGoalFailed();
        }

        private void SetUpRingCamera()
        {
            Transform playerTransform = _throwingPlayer.transform;
            Transform ringTransform = _ring.transform;
            CinemachineVirtualCamera ringCamera = _ring.VirtualCamera;
            _ring.RingTargetGroup.m_Targets[_cameraTargetGroupPlayerIndex].target = playerTransform;
            ringCamera.Follow = playerTransform;
            ringCamera.LookAt = _ring.RingTargetGroup.Transform;
            CinemachineFramingTransposer framingTransposer =
                ringCamera.GetCinemachineComponent<CinemachineFramingTransposer>()
                ?? throw new NullReferenceException("There is no framingTransposer on RingVirtualCamera");

            framingTransposer.m_TrackedObjectOffset.x =
                ringTransform.position.z > playerTransform.position.z != _ring.IsFlipped
                    ? -_defaultCameraXOffset
                    : _defaultCameraXOffset;

            ringCamera.Prioritize();
        }

        private void SubscribeOnEnemyRing() =>
            _ring.Goal += OnGoalScored;

        private void UnsubscribeFromEnemyRing() =>
            _ring.Goal -= OnGoalScored;

        private void OnGoalFailed() =>
            Lost?.Invoke();

        private void OnGoalScored() =>
            Won?.Invoke();
    }
}