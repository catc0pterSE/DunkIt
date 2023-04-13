using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;

namespace UI.HUD.Controls.StateMachine.States
{
    public class DropBallState : IParameterState<PlayerFacade>
    {
        private readonly IControlsHUDView _controlsHUDView;
        private PlayerFacade _player;

        public DropBallState(IControlsHUDView controlsHUDView)
        {
            _controlsHUDView = controlsHUDView;
        }
        
        public void Enter(PlayerFacade player)
        {
            
            Debug.Log("drop state");
            _controlsHUDView.Enable();
            _player = player;
            SubscribeHudOnCurrentPlayer();
        }

        public void Exit()
        {
            _controlsHUDView.Disable();
            UnsubscribeHudFromCurrentPlayer();
            _controlsHUDView.SetPassAvailability(false);
        }
        
        private void SubscribeHudOnCurrentPlayer()
        {
            _player.PassReached += _controlsHUDView.SetPassAvailability;
        }

        private void UnsubscribeHudFromCurrentPlayer()
        {
            _player.PassReached -= _controlsHUDView.SetPassAvailability;
        }
    }
}