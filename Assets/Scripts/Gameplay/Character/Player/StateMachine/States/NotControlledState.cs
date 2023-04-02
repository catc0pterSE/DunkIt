using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class NotControlledState : IParameterlessState
    {
        private readonly PlayerFacade _player;

        public NotControlledState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            
        }
        
        public void Exit()
        {
            
        }
    }
}