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
        }

        public void Exit()
        {
            _controlsHUDView.Disable();
            _controlsHUDView.SetChangePlayerAvailability(false);
            _controlsHUDView.SetMovementAvailability(false);
        }
    }
}