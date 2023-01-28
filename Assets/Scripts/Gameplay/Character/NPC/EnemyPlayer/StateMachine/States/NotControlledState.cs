using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.NPC.EnemyPlayer.StateMachine.States
{
    public class NotControlledState : IParameterlessState
    {
        private readonly EnemyFacade _enemyFacade;

        public NotControlledState(EnemyFacade enemyFacade)
        {
            _enemyFacade = enemyFacade;
        }


        public void Enter()
        {
            
        }
        
        public void Exit()
        {
           
        }
    }
}