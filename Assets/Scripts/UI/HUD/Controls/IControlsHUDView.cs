using Modules.MonoBehaviour;
using UI.HUD.Controls.StateMachine;

namespace UI.HUD.Controls
{
    public interface IControlsHUDView : ISwitchable
    {
        public void SetThrowAvailability(bool isAvailable);

        public void SetDunkAvailability(bool isAvailable);

        public void SetPassAvailability(bool isAvailable);

        public void SetChangePlayerAvailability(bool isAvailable);
        
        public void SetMovementAvailability(bool isAvailable);
        
        public void SetJumpAvailability(bool isAvailable);

        public IControlsHUDView Initialize(IControlsHUDStateController controlsHUDStateController);
    }
}