using Gameplay.NPC.EnemyPlayer.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.NPC.EnemyPlayer.StateMachine.States
{
    public class CutsceneState : IParameterlessState
    {
        private readonly EnemyFacade _enemyFacade;

        public CutsceneState(EnemyFacade enemyFacade)
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