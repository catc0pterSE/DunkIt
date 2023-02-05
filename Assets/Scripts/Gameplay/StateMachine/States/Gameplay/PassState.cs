using System;
using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class PassState : IParameterlessState
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

        public void Enter()
        {
            _passingPlayer = _playerTeam.FindFirstOrNull(player => player.OwnsBall)
                             ?? throw new NullReferenceException("No players owns ball");
            _catchingPlayer = _playerTeam.FindFirstOrNull(player => player != _passingPlayer);
            SetPlayersStates();
            _catchingPlayer.CaughtBall += MoveToGameplayState;
        }

        private void SetPlayersStates()
        {
            _passingPlayer.StateMachine.Enter<Character.Player.StateMachine.States.PassState>();
            _catchingPlayer.StateMachine.Enter<Character.Player.StateMachine.States.CatchState>();
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