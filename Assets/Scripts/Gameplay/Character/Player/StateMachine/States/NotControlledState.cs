using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class NotControlledState : IParameterlessState
    {
        private readonly PlayerFacade _playerFacade;

        public NotControlledState(PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
        }

        public void Enter()
        {
            _playerFacade.DisablePlayerMover();
            _playerFacade.DisableInputControlledBrain();
            _playerFacade.DisableAIControlledBrain();
        }
        
        public void Exit()
        {
            
        }
    }
}