using Gameplay.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Player.StateMachine.States
{
    public class AIState : IParameterlessState
    {
        private readonly PlayerFacade _playerFacade;

        public AIState(PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
        }
        
        public void Enter() =>
            _playerFacade.EnableAIControlledBrain();
        
        public void Exit() =>
            _playerFacade.DisableAIControlledBrain();
    }
}