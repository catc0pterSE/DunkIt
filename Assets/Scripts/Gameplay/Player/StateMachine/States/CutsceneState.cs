using Gameplay.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Player.StateMachine.States
{
    public class CutsceneState : IParameterlessState
    {
        private readonly PlayerFacade _playerFacade;

        public CutsceneState(PlayerFacade playerFacade)
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