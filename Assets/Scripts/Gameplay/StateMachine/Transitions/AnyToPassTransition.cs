using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.Transitions
{
    public class AnyToPassTransition : ITransition
    {
        private readonly PlayerFacade[] _players;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public AnyToPassTransition(PlayerFacade[] players,
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
            _players.Map(player => player.PassInitiated += MoveToPassState);

        private void UnsubscribeFromPlayers() =>
            _players.Map(player => player.PassInitiated -= MoveToPassState);

        private void MoveToPassState(PlayerFacade passingPlayer, PlayerFacade passTarget) =>
            _gameplayLoopStateMachine.Enter<PassState, PlayerFacade, PlayerFacade>(passingPlayer, passTarget);
    }
}