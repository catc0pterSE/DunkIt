using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.Transitions
{
    public class GameplayStateToDunkStateTransition : ITransition
    {
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly PlayerFacade[] _playerTeam;

        public GameplayStateToDunkStateTransition(GameplayLoopStateMachine gameplayLoopStateMachine,
            PlayerFacade[] playerTeam)
        {
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _playerTeam = playerTeam;
        }

        public void Enable() => SubscribeOnPlayers();

        public void Disable() => UnsubscribeFromPlayers();


        private void SubscribeOnPlayers() =>
            _playerTeam.Map(player => player.DunkInitiated += MoveToDunkState);

        private void UnsubscribeFromPlayers() =>
            _playerTeam.Map(player => player.DunkInitiated -= MoveToDunkState);

        private void MoveToDunkState(PlayerFacade dunkingPlayer) =>
            _gameplayLoopStateMachine.Enter<DunkState, PlayerFacade>(dunkingPlayer);
    }
}