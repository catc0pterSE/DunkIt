using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.StateMachine.States.MiniGameStates
{
    public class RefereeBallState: IParameterlessState
    {
        public void Enter()
        {
            Debug.Log("Entered referee ball");
        }
        
        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}