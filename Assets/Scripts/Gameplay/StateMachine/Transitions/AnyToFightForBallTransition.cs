using System.Collections;
using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.MinigameStates;
using Infrastructure.CoroutineRunner;
using Modules.StateMachine;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.StateMachine.Transitions
{
    public class AnyToFightForBallTransition : ITransition
    {
        private readonly PlayerFacade[] _players;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly float _delaySeconds;

        private Coroutine _delayRoutine;
        private bool _isOnDelay;

        public AnyToFightForBallTransition(PlayerFacade[] players, GameplayLoopStateMachine gameplayLoopStateMachine,
            ICoroutineRunner coroutineRunner, float delaySeconds = 0)
        {
            _players = players;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _coroutineRunner = coroutineRunner;
            _delaySeconds = delaySeconds;
        }

        public void Enable()
        {
            StartDelay();
            SubscribeOnPlayers();
        }

        public void Disable() =>
            UnsubscribeFromPlayers();

        private void StartDelay()
        {
            _isOnDelay = false;
            
            if (_delayRoutine != null)
                _coroutineRunner.StopCoroutine(_delayRoutine);

            _delayRoutine = _coroutineRunner.StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            _isOnDelay = true;
            yield return new WaitForSeconds(_delaySeconds);
            _isOnDelay = false;
        }

        private void SubscribeOnPlayers() =>
            _players.Map(player => player.FightForBallStarted += OnFightForBallInitiated);


        private void UnsubscribeFromPlayers() =>
            _players.Map(player => player.FightForBallStarted -= OnFightForBallInitiated);

        private void OnFightForBallInitiated(PlayerFacade[] participants)
        {
            if (_isOnDelay == false)
                _gameplayLoopStateMachine.Enter<FightForBallState, PlayerFacade[]>(participants);
        }
    }
}