using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    public class CelebrateCutsceneState : IParameterlessState
    {
        public void Exit()
        {
           
        }

        public void Enter()
        {
            Debug.Log("Celebrate cutscene state");
        }
    }
}