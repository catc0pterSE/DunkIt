using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class AIControlledState : IParameterlessState
    {
        private readonly PlayerFacade _playerFacade;

        public AIControlledState(PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
        }

        public void Enter()
        {
            _playerFacade.EnableAIControlledBrain();
            _playerFacade.EnablePlayerMover();
            _playerFacade.DeprioritizeCamera();
        }


        public void Exit()
        {
            _playerFacade.DisableAIControlledBrain();
            _playerFacade.DisablePlayerMover();
        }
            
    }
}