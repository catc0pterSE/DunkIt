using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;

namespace UI.HUD.Controls.StateMachine.States
{
    public class BallChasingState : IParameterlessState
    {
        private readonly IControlsHUDView _controlsHUDView;

        public BallChasingState(IControlsHUDView controlsHUDView)
        {
            _controlsHUDView = controlsHUDView;
        }

        public void Enter()
        {
            _controlsHUDView.Enable();
            _controlsHUDView.SetChangePlayerAvailability(true);
            _controlsHUDView.SetMovementAvailability(true);
            _controlsHUDView.SetJumpAvailability(true);
        }

        public void Exit()
        {
            _controlsHUDView.SetChangePlayerAvailability(false);
            _controlsHUDView.SetMovementAvailability(false);
            _controlsHUDView.SetJumpAvailability(false);
            _controlsHUDView.Disable();
        }
    }
}