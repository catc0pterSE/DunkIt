using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class PassState : IParameterState<PlayerFacade>
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly PlayerFacade[] _enemyTeam;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        private PlayerFacade _passingPlayer;
        private PlayerFacade _catchingPlayer;

        public PassState(PlayerFacade[] playerTeam, PlayerFacade[] enemyTeam,
            GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enter(PlayerFacade passingPlayer)
        {
            _passingPlayer = passingPlayer;
            _catchingPlayer = _playerTeam.FindFirstOrNull(player => player != _passingPlayer);
            SetPlayersStates();
            SubscribeOnCatchingPlayers();
        }

        private void SubscribeOnCatchingPlayers()
        {
            _catchingPlayer.CaughtBall += MoveToGameplayState;
            _enemyTeam.Map(enemy => enemy.CaughtBall += MoveToGameplayState);
        }

        private void UnsubscribeFromCatchingPlayers()
        {
            _catchingPlayer.CaughtBall -= MoveToGameplayState;
            _enemyTeam.Map(enemy => enemy.CaughtBall -= MoveToGameplayState);
        }

        private void SetPlayersStates()
        {
            _catchingPlayer.EnterCatchState();
            _enemyTeam.Map(enemy => enemy.EnterCatchState());
            _passingPlayer.EnterPassState();
        }

        private void MoveToGameplayState()
        {
            _gameplayLoopStateMachine.Enter<GameplayState>();
        }

        public void Exit()
        {
           UnsubscribeFromCatchingPlayers();
        }
    }
}