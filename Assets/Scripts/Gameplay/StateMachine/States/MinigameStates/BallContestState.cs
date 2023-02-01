using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public class BallContestState: IParameterState<(PlayerFacade, EnemyFacade)>
    {
        public void Enter((PlayerFacade, EnemyFacade) payLoad)
        {
        }

        public void Exit()
        {
          
        }
    }
}