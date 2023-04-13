using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;

namespace UI.HUD.Controls.StateMachine.States
{
    public class AttackWithBallState : IParameterState<PlayerFacade>
    {
        private readonly IControlsHUDView _controlsHUDView;
        private PlayerFacade _player;

        public AttackWithBallState(IControlsHUDView controlsHUDView)
        {
            _controlsHUDView = controlsHUDView;
        }
        
        public void Enter(PlayerFacade payload)
        {
            _controlsHUDView.Enable();
            _player = payload;
            _controlsHUDView.SetMovementAvailability(true);
            SubscribeHudOnCurrentPlayer();
        }

        public void Exit()
        {
            _controlsHUDView.Disable();
            UnsubscribeHudFromCurrentPlayer();
            _controlsHUDView.SetDunkAvailability(false);
            _controlsHUDView.SetPassAvailability(false);
            _controlsHUDView.SetThrowAvailability(false);
            _controlsHUDView.SetMovementAvailability(false);
        }
        
        private void SubscribeHudOnCurrentPlayer()
        {
            _player.ThrowReached += _controlsHUDView.SetThrowAvailability;
            _player.DunkReached += _controlsHUDView.SetDunkAvailability;
            _player.PassReached += _controlsHUDView.SetPassAvailability;
        }

        private void UnsubscribeHudFromCurrentPlayer()
        {
            _player.ThrowReached -= _controlsHUDView.SetThrowAvailability;
            _player.DunkReached -= _controlsHUDView.SetDunkAvailability;
            _player.PassReached -= _controlsHUDView.SetPassAvailability;
        }
    }
}