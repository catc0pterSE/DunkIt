using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;

namespace UI.HUD.Controls.StateMachine.States
{
    public class OffState: IParameterlessState
    {
        private readonly IControlsHUDView _controlsHUDView;

        public OffState(IControlsHUDView controlsHUDView)
        {
            _controlsHUDView = controlsHUDView;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}