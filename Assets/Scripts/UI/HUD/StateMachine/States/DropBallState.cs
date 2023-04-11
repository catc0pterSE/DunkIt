using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace UI.HUD.StateMachine.States
{
    public class DropBallState : IParameterState<PlayerFacade>
    {
        private readonly IGameplayHUD _gameplayHUD;
        private PlayerFacade _player;

        public DropBallState(IGameplayHUD gameplayHUD)
        {
            _gameplayHUD = gameplayHUD;
        }
        
        public void Enter(PlayerFacade payload)
        {
            _player = payload;
            SubscribeHudOnCurrentPlayer();
        }

        public void Exit()
        {
            UnsubscribeHudFromCurrentPlayer();
            _gameplayHUD.SetPassAvailability(false);
        }
        
        private void SubscribeHudOnCurrentPlayer()
        {
            _player.PassReached += _gameplayHUD.SetPassAvailability;
        }

        private void UnsubscribeHudFromCurrentPlayer()
        {
            _player.PassReached -= _gameplayHUD.SetPassAvailability;
        }
    }
}