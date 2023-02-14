using System;
using System.Collections;
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
            PrioritizeRingCamera();
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
            _throwingPlayer.PrioritizeCamera();
            _throwingPlayer.DisableBallThrower();
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

        private void PrioritizeRingCamera() =>
            _sceneConfig.EnemyRing.VirtualCamera.Prioritize();

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