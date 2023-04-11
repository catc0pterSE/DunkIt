using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Gameplay.StateMachine.States.MinigameStates;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.Transitions
{
    public class AnyToThrowStateTransition : ITransition
    {
        private readonly PlayerFacade[] _players;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public AnyToThrowStateTransition(PlayerFacade[] players,
            GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _players = players;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable() =>
            SubscribeOnPlayers();

        public void Disable() =>
            UnsubscribeFromPlayers();

        private void SubscribeOnPlayers() =>
            _players.Map(player => player.ThrowInitiated += MoveToThrowState);

        private void UnsubscribeFromPlayers() =>
            _players.Map(player => player.ThrowInitiated -= MoveToThrowState);

        private void MoveToThrowState(PlayerFacade throwingPlayer) =>
            _gameplayLoopStateMachine.Enter<ThrowState, PlayerFacade>(throwingPlayer);
    }
}