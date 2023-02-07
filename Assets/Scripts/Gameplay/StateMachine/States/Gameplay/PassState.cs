using System;
using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class PassState : IParameterState<PlayerFacade>
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        private PlayerFacade _passingPlayer;
        private PlayerFacade _catchingPlayer;

        public PassState(PlayerFacade[] playerTeam, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _playerTeam = playerTeam;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enter(PlayerFacade passingPlayer)
        {
            _passingPlayer = passingPlayer;
            _catchingPlayer = _playerTeam.FindFirstOrNull(player => player != _passingPlayer);
            SetPlayersStates();
            _catchingPlayer.CaughtBall += MoveToGameplayState;
        }

        private void SetPlayersStates()
        {
            _passingPlayer.EnterPassState();
            _catchingPlayer.EnterCatchState();
        }

        private void MoveToGameplayState()
        {
            _gameplayLoopStateMachine.Enter<GameplayState>();
        }

        public void Exit()
        {
            _catchingPlayer.CaughtBall -= MoveToGameplayState;
        }
    }
}