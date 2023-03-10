using System.Linq;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.Transitions
{
    public class GameplayStateToPassTransition : ITransition
    {
        private readonly PlayerFacade[] _players;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public GameplayStateToPassTransition(PlayerFacade[] playerTeam, PlayerFacade[] enemyTeam,
            GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _players = playerTeam.Union(enemyTeam).ToArray();
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable() => SubscribeOnPlayers();


        public void Disable() => UnsubscribeFromPlayers();

        private void SubscribeOnPlayers() =>
            _players.Map(player => player.PassInitiated += MoveToPassState);

        private void UnsubscribeFromPlayers() =>
            _players.Map(player => player.PassInitiated -= MoveToPassState);

        private void MoveToPassState(PlayerFacade passingPlayer) =>
            _gameplayLoopStateMachine.Enter<PassState, PlayerFacade>(passingPlayer);
    }
}