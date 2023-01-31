using System;
using System.Collections;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Modules.MonoBehaviour;
using Scene;
using UI;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.Minigame.Throw
{
    using Ball.MonoBehavior;

    public class ThrowMinigame : SwitchableMonoBehaviour, IMinigame
    {
        [SerializeField] private ThrowUI _interface;

        private readonly WaitForSeconds _waitForGoalTrackingTime =
            new WaitForSeconds(NumericConstants.BallTrackingSeconds);
        
        private PlayerFacade _throwingPlayer;
        private EnemyFacade _primaryEnemy;
        private SceneConfig _sceneConfig;
        private Ball _ball;
        private LoadingCurtain _loadingCurtain;
        private Coroutine _trackingGoal;

        public event Action Won;
        public event Action Lost;

        public IMinigame Initialize
        (
            PlayerFacade throwingPlayer,
            EnemyFacade primaryEnemy,
            SceneConfig sceneConfig,
            Ball ball,
            LoadingCurtain loadingCurtain
        )
        {
            _throwingPlayer = throwingPlayer;
            _primaryEnemy = primaryEnemy;
            _sceneConfig = sceneConfig;
            _ball = ball;
            _loadingCurtain = loadingCurtain;

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
            _interface.Disable();
        }
    }
}