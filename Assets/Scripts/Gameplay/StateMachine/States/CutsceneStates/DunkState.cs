using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    public class DunkState : IParameterlessState
    {
        public void Exit()
        {
        }

        public void Enter()
        {
            Debug.Log("Entered Dunk State");
        }
    }
}