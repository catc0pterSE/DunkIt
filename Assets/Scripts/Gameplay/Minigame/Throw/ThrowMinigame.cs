using System;
using System.Collections;
using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour;
using Modules.MonoBehaviour;
using Scene;
using UI;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Minigame.Throw
{
    public class ThrowMinigame : SwitchableMonoBehaviour, IMinigame
    {
        [SerializeField] private ThrowUI _interface;
        [SerializeField] private float _ballTrackingSeconds = 5;
        [SerializeField] private float _defaultCameraXOffset = 10;
        [SerializeField] private int _cameraTargetGroupPlayerIndex = 1;

        private WaitForSeconds _waitForGoalTrackingTime;
        private PlayerFacade _throwingPlayer;
        private PlayerFacade _primaryEnemy;
        private SceneConfig _sceneConfig;
        private Ball.MonoBehavior.Ball _ball;
        private LoadingCurtain _loadingCurtain;
        private Coroutine _trackingGoal;

        public event Action Won;
        public event Action Lost;

        public IMinigame Initialize
        (
            PlayerFacade throwingPlayer,
            PlayerFacade primaryEnemy,
            SceneConfig sceneConfig,
            Ball.MonoBehavior.Ball ball,
            LoadingCurtain loadingCurtain
        )
        {
            _throwingPlayer = throwingPlayer;
            _primaryEnemy = primaryEnemy;
            _sceneConfig = sceneConfig;
            _ball = ball;
            _loadingCurtain = loadingCurtain;
            _waitForGoalTrackingTime = new WaitForSeconds(_ballTrackingSeconds);

            return this;
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
            _sceneConfig.EnemyRing.VirtualCamera.LookAt = _ball.transform;
            StartGoalTracking();
        }

        private void StartGoalTracking()
        {
            StopBallTracking();
            _trackingGoal = StartCoroutine(TrackGoal());
        }

        private void StopBallTracking()
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
            Transform ringTransform = _sceneConfig.EnemyRing.transform;
            CinemachineVirtualCamera ringCamera = _sceneConfig.EnemyRing.VirtualCamera;
            _sceneConfig.EnemyRing.RingTargetGroup.m_Targets[_cameraTargetGroupPlayerIndex].target = playerTransform;
            ringCamera.Follow = playerTransform;
            CinemachineFramingTransposer framingTransposer =
                ringCamera.GetCinemachineComponent<CinemachineFramingTransposer>()
                ?? throw new NullReferenceException("There is no framingTransposer on RingVirtualCamera");
            framingTransposer.m_TrackedObjectOffset.x =
                ringTransform.position.z > playerTransform.position.z ? -_defaultCameraXOffset : _defaultCameraXOffset;
            ringCamera.Prioritize();
        }

        private void SubscribeOnEnemyRing() =>
            _sceneConfig.EnemyRing.Goal += OnGoalScored;

        private void UnsubscribeFromEnemyRing() =>
            _sceneConfig.EnemyRing.Goal -= OnGoalScored;

        private void OnGoalFailed()
        {
            StopBallTracking();
            _loadingCurtain.FadeInFadeOut(SetDropBall);
        }

        private void SetDropBall()
        {
            _primaryEnemy.transform.position = _sceneConfig.EnemyDropBallPoint.position;
            _ball.SetOwner(_primaryEnemy);
            Finish();
            Lost?.Invoke();
        }

        private void OnGoalScored()
        {
            Finish();
            Won?.Invoke();
        }

        private void Finish()
        {
            StopBallTracking();
            UnsubscribeFromThrowingPlayer();
        }
    }
}