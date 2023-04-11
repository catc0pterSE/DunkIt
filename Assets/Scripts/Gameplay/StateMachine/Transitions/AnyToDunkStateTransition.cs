using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.Transitions
{
    public class AnyToDunkStateTransition : ITransition
    {
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly PlayerFacade[] _players;

        public AnyToDunkStateTransition(PlayerFacade[] players, GameplayLoopStateMachine gameplayLoopStateMachine)

        {
            _players = players;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable() => SubscribeOnPlayers();

        public void Disable() => UnsubscribeFromPlayers();


        private void SubscribeOnPlayers() =>
            _players.Map(player => player.DunkInitiated += OnPlayerDunk);

        private void UnsubscribeFromPlayers() =>
            _players.Map(player => player.DunkInitiated -= OnPlayerDunk);

        private void OnPlayerDunk(PlayerFacade dunkingPlayer) =>
            _gameplayLoopStateMachine.Enter<DunkState, PlayerFacade>(dunkingPlayer);
    }
}