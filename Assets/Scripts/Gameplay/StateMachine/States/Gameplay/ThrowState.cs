using System.Collections;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.EnemyPlayer.StateMachine.States;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.CutsceneStates;
using Infrastructure.CoroutineRunner;
using Modules.StateMachine;
using Scene;
using UI;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class ThrowState : IParameterState<PlayerFacade>
    {
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly EnemyFacade[] _enemyTeam;
        private readonly SceneConfig _sceneConfig;
        private readonly WaitForSeconds _goalTrackingTime = new WaitForSeconds(NumericConstants.BallTrackingSeconds);

        private PlayerFacade _throwingPlayer;
        private Coroutine _trackingGoal;
       
        public ThrowState(SceneConfig sceneConfig, GameplayLoopStateMachine gameplayLoopStateMachine,
            EnemyFacade[] enemyTeam, Ball.MonoBehavior.Ball ball, ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
        {
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _ball = ball;
            _coroutineRunner = coroutineRunner;
            _loadingCurtain = loadingCurtain;
            _enemyTeam = enemyTeam;
            _sceneConfig = sceneConfig;
        }

        public void Enter(PlayerFacade player)
        {
            SetThrowingPlayer(player);
            SetThrowingPlayerState();
            SetEnemiesStates();
            PrioritizeRingCamera();
            SubscribeOnThrowingPlayer();
        }

        public void Exit()
        {
            DeprioritizeRingCamera();
            _throwingPlayer.DeprioritizeCamera();
            UnsubscribeFromThrowingPlayer();
        }
        
        private void PrioritizeRingCamera()=>
            _sceneConfig.EnemyRing.VirtualCamera.Prioritize();
        
        private void DeprioritizeRingCamera()=>
            _sceneConfig.EnemyRing.VirtualCamera.Deprioritize();

        private void SubscribeOnThrowingPlayer() =>
            _throwingPlayer.BallThrown += OnBallThrown;


        private void UnsubscribeFromThrowingPlayer() =>
            _throwingPlayer.BallThrown -= OnBallThrown;

        private void OnBallThrown()
        {
            DeprioritizeRingCamera();
            StartGoalTracking();
        }

        private void StartGoalTracking()
        {
            if (_trackingGoal != null)
                _coroutineRunner.StopCoroutine(_trackingGoal);

            _trackingGoal = _coroutineRunner.StartCoroutine(TrackGoal());
        }

        private IEnumerator TrackGoal()
        {
            SubscribeOnEnemyRing();
            yield return _goalTrackingTime;
            UnsubscribeFromEnemyRing();
            OnGoalFailed();
        }

        private void SetThrowingPlayer(PlayerFacade player)
        {
            _throwingPlayer = player;
        }

        private void SetThrowingPlayerState() =>
            _throwingPlayer.StateMachine.Enter<Character.Player.StateMachine.States.ThrowState>();

        private void SetEnemiesStates()
        {
            _enemyTeam.Map(enemy => enemy.StateMachine.Enter<NotControlledState>());
        }

        private void SubscribeOnEnemyRing() =>
            _sceneConfig.EnemyRing.Goal += OnGoalScored;

        private void UnsubscribeFromEnemyRing() =>
            _sceneConfig.EnemyRing.Goal -= OnGoalScored;

        private void OnGoalScored()
        {
            _coroutineRunner.StopCoroutine(_trackingGoal);
            MoveToCelebrateCutsceneState();
        }

        private void OnGoalFailed()
        {
            _coroutineRunner.StopCoroutine(_trackingGoal);
            _loadingCurtain.FadeInFadeOut(MoveToGameplayState);
        }

        private void MoveToCelebrateCutsceneState() =>
            _gameplayLoopStateMachine.Enter<CelebrateCutsceneState>();

        private void MoveToGameplayState()
        {
            EnemyFacade primaryEnemy = _enemyTeam[NumericConstants.PrimaryTeamMemberIndex];
            primaryEnemy.transform.position = _sceneConfig.EnemyDropBallPoint.position;
            _ball.SetOwner(primaryEnemy);
            _gameplayLoopStateMachine.Enter<GameplayState>();
        }
            
    }
}