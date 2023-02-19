using System.Collections;
using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.MinigameStates;
using Infrastructure.CoroutineRunner;
using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.StateMachine.Transitions
{
    public class AnyToFightForBallTransition : ITransition
    {
        private const float DelayTime = 5;

        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly bool _isDelayable;
        private readonly WaitForSeconds _waitForDelay = new WaitForSeconds(DelayTime);

        private PlayerFacade _currentBallOwner;
        private Coroutine _delayRoutine;
        private bool _isOnDelay;

        public AnyToFightForBallTransition(Ball.MonoBehavior.Ball ball,
            GameplayLoopStateMachine gameplayLoopStateMachine, ICoroutineRunner coroutineRunner, bool isDelayable)
        {
            _ball = ball;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _coroutineRunner = coroutineRunner;
            _isDelayable = isDelayable;
        }

        public void Enable()
        {
            _isOnDelay = false;
            
            if (_isDelayable)
                StartDelay();
            
            SubscribeOnBall();
            OnBallOwnerChanged(_ball.Owner);
        }

        public void Disable()
        {
            UnsubscribeFromBall();
            UnsubscribeFromCurrentBallOwner();
        }

        private void SubscribeOnBall() =>
            _ball.OwnerChanged += OnBallOwnerChanged;

        private void UnsubscribeFromBall() =>
            _ball.OwnerChanged -= OnBallOwnerChanged;

        private void OnBallOwnerChanged(CharacterFacade newOwner)
        {
            UnsubscribeFromCurrentBallOwner();

            if (newOwner is not PlayerFacade basketballPlayer)
                return;

            _currentBallOwner = basketballPlayer;
            SubscribeOnCurrentBallOwner();
        }

        private void StartDelay()
        {
            if (_delayRoutine != null)
                _coroutineRunner.StopCoroutine(_delayRoutine);

            _delayRoutine = _coroutineRunner.StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            _isOnDelay = true;
            yield return _waitForDelay;
            _isOnDelay = false;
        }

        private void SubscribeOnCurrentBallOwner() =>
            _currentBallOwner.BallContestStarted += MoveToBallContestState;


        private void UnsubscribeFromCurrentBallOwner()
        {
            if (_currentBallOwner != null)
                _currentBallOwner.BallContestStarted -= MoveToBallContestState;
        }


        private void MoveToBallContestState(PlayerFacade player, PlayerFacade enemy)
        {
            if (_isOnDelay == false)
                _gameplayLoopStateMachine.Enter<BallContestState, (PlayerFacade, PlayerFacade)>((player, enemy));
        }
    }
}