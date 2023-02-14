using System.Collections;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.CutsceneStates;
using Gameplay.StateMachine.States.Gameplay;
using Infrastructure.CoroutineRunner;
using Modules.StateMachine;
using Scene;
using Scene.Ring;
using UI;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.StateMachine.Transitions
{
    public class GameplayStateToUpsetCutsceneStateTransition: ITransition
    {
        private const float BallTrackingSeconds = 5;
        
        private readonly PlayerFacade[] _playerTeam;
        private readonly PlayerFacade[] _enemyTeam;
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly Ring _enemyRing;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly SceneConfig _sceneConfig;
        private readonly WaitForSeconds _goalTrackingTime = new WaitForSeconds(BallTrackingSeconds);
        
        private Coroutine _trackingGoal;
        
        public GameplayStateToUpsetCutsceneStateTransition
            (
                PlayerFacade[] playerTeam,
                PlayerFacade[] enemyTeam,
                Ball.MonoBehavior.Ball ball,
                Ring enemyRing,
                LoadingCurtain loadingCurtain,
                GameplayLoopStateMachine gameplayLoopStateMachine,
                ICoroutineRunner coroutineRunner,
                SceneConfig sceneConfig
            )
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _ball = ball;
            _enemyRing = enemyRing;
            _loadingCurtain = loadingCurtain;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _coroutineRunner = coroutineRunner;
            _sceneConfig = sceneConfig;
        }
        
        public void Enable()
        {
            SubscribeOnEnemies();
        }
        
        public void Disable()
        {
            UnsubscribeFromEnemies();
        }
        
        private void SubscribeOnEnemies()=>
            _enemyTeam.Map(enemy => enemy.BallThrown += OnEnemyThrownBall);
        
        private void UnsubscribeFromEnemies()=>
            _enemyTeam.Map(enemy => enemy.BallThrown -= OnEnemyThrownBall);

        private void OnEnemyThrownBall()
        {
            StopGoalTracking();
            _trackingGoal = _coroutineRunner.StartCoroutine(TrackGoal());
        }

        private IEnumerator TrackGoal()
        {
            SubscribeOnEnemyRing();
            yield return _goalTrackingTime;
            UnsubscribeFromEnemyRing();
            OnThrowFailed();
        }

        private void SubscribeOnEnemyRing() =>
            _enemyRing.Goal += OnGoalScored;

        private void UnsubscribeFromEnemyRing()=>
            _enemyRing.Goal -= OnGoalScored;

        private void OnThrowFailed()
        {
            StopGoalTracking();
            _loadingCurtain.FadeInFadeOut(MoveToGameplayState);
        }

        private void OnGoalScored()
        {
           StopGoalTracking();
           _gameplayLoopStateMachine.Enter<UpsetCutsceneState>();
        }

        private void StopGoalTracking()
        {
            if (_trackingGoal!=null)
                _coroutineRunner.StopCoroutine(_trackingGoal);
        }

        private void MoveToGameplayState()
        {
            PlayerFacade primaryPlayer = _playerTeam[NumericConstants.PrimaryTeamMemberIndex];
            _ball.SetOwner(primaryPlayer);
            primaryPlayer.transform.position = _sceneConfig.PlayerDropBallPoint.position;
            _gameplayLoopStateMachine.Enter<GameplayState>();
        }
    }
}