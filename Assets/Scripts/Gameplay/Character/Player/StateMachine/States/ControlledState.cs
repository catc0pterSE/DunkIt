using Gameplay.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Player.StateMachine.States
{
    public class ControlledState : IParameterlessState
    {
        private readonly PlayerFacade _playerFacade;

        public ControlledState(PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
        }
        
        public void Enter() =>
            _playerFacade.EnableInputControlledBrain();
        
        public void Exit() =>
            _playerFacade.DisableInputControlledBrain();
        
    }
}