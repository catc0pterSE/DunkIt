using System.Linq;
using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class PassState : IParameterState<PlayerFacade>
    {
        private readonly PlayerFacade[] _players;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        private PlayerFacade _passingPlayer;
        private PlayerFacade[] _catchingPlayers;

        public PassState(PlayerFacade[] playerTeam, PlayerFacade[] enemyTeam,
            GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _players = playerTeam.Union(enemyTeam).ToArray();
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enter(PlayerFacade passingPlayer)
        {
            _passingPlayer = passingPlayer;
            _catchingPlayers = _players.Where(player => player != _passingPlayer).ToArray();

            SetPlayersStates();
            SubscribeOnCatchingPlayers();
        }

        private void SubscribeOnCatchingPlayers() =>
            _catchingPlayers.Map(player => player.CaughtBall += MoveToGameplayState);


        private void UnsubscribeFromCatchingPlayers() =>
            _catchingPlayers.Map(player => player.CaughtBall -= MoveToGameplayState);


        private void SetPlayersStates()
        {
            _passingPlayer.EnterPassState();
            _catchingPlayers.Map(player => player.EnterCatchState(_passingPlayer));
        }

        private void MoveToGameplayState() =>
            _gameplayLoopStateMachine.Enter<GameplayState>();


        public void Exit() =>
            UnsubscribeFromCatchingPlayers();
    }
}