using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.MinigameStates;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.Transitions
{
    public class GameplayStateToThrowStateTransition : ITransition
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public GameplayStateToThrowStateTransition(PlayerFacade[] playerTeam,
            GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _playerTeam = playerTeam;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable() => SubscribeOnPlayers();

        public void Disable() => UnsubscribeFromPlayers();

        private void SubscribeOnPlayers() =>
            _playerTeam.Map(player => player.ThrowInitiated += MoveToThrowState);

        private void UnsubscribeFromPlayers() =>
            _playerTeam.Map(player => player.ThrowInitiated -= MoveToThrowState);

        private void MoveToThrowState(PlayerFacade throwingPlayer) =>
            _gameplayLoopStateMachine.Enter<ThrowState, PlayerFacade>(throwingPlayer);
    }
}