using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.NPC.EnemyPlayer.StateMachine.States
{
    public class NotControlledState : IParameterlessState
    {
        private readonly EnemyFacade _enemy;

        public NotControlledState(EnemyFacade enemy)
        {
            _enemy = enemy;
        }


        public void Enter()
        {
            
        }
        
        public void Exit()
        {
           
        }
    }
}