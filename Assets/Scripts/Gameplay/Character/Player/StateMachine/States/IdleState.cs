using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class IdleState: IParameterlessState
    {
        public IdleState(PlayerFacade player)
        {
        }
        
        public void Enter()
        {
            //play idle animation
        }

        public void Exit()
        {
        }
    }
}