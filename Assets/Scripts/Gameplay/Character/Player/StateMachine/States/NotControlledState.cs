using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class NotControlledState : IParameterlessState
    {
        private readonly MonoBehaviour.PlayerFacade _player;

        public NotControlledState(MonoBehaviour.PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.DisablePlayerMover();
            _player.DisableInputControlledBrain();
            _player.DisableAIControlledBrain();
        }
        
        public void Exit()
        {
            
        }
    }
}