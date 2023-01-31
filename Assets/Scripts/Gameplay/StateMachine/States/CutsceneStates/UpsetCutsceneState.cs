using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    public class UpsetCutsceneState : IParameterlessState
    {
        public void Exit()
        {
            
        }

        public void Enter()
        {
            Debug.Log("Upset cutscene state");
        }
    }
}